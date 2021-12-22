#!/usr/bin/env lua

-- Targets premake script.

newoption { trigger = "prefix", value = "/usr/local",
	description = "Installation prefix"}
newoption { trigger = "buildtest", value = "ON",
	allowed = { { "ON", "Enable build test" }, { "OFF", "Disable build test" } },
	description = "Build test(s)"}

newoption { trigger = "fmts", value = "tar.gz,zip",
	description = "Select archive format(s)"}

newoption { trigger = "framework", value = "net471",
	description = "Target framework"}

newaction { trigger = "help", description = "Help - displays targets",
	execute = function ()
		os.execute("gmake help")
	end
}

newaction { trigger = "all", description = "Compile the software",
	execute = function ()
		os.execute("gmake")
	end
}

--testconsoleapp = os.matchfiles("$$HOME/.nuget/packages/xunit.runner.console/*/tools/net452/xunit.console.exe")[1]
--testappargs = "-nologo -appdomains denied -xml TestResult.xml"
testconsoleapp = os.matchfiles("$$HOME/.nuget/packages/nunit.consolerunner/*/tools/nunit3-console.exe")[1]
testappargs = "--noheader --domain=None --labels=OnOutputOnly --output=testout.txt \"--result=TestResult.xml;format=nunit3\""

if "ON" == (_OPTIONS["buildtest"] or "ON") then
	newaction { trigger = "test", description = "Test the software",
		execute = function ()
			os.execute(string.format("LD_LIBRARY_PATH=$$LD_LIBRARY_PATH:. MONO_PATH=$$MONO_PATH:. MONO_GAC_PREFIX=$$HOME/.local %s %s %s %s.Tests.dll %s",
			  exelauncher, testconsoleapp, testappargs, solution().name,
				#_ARGS ~= 0 and table.concat(_ARGS, " ") or ""))
		end
	}
end

newaction { trigger = "nugetadd", description = "Nuget add artifacts",
	execute = function ()
		local wsp = solution()
		os.execute(string.format("%s $$HOME/bin/nuget.exe add -source $$HOME/.nuget/packages $$TARGETDIR/%s.%s.nupkg",
		  exelauncher, wsp.name, wsp.version))
	end
}

newaction { trigger = "nugetinstall", description = "Nuget install artifacts",
	execute = function ()
		local wsp = solution()
		--[=[os.copyfile("bin/myprogram", string.format("%s/bin/myprogram",
			_OPTIONS["prefix"] or "/usr/local"))]=]
		os.execute(string.format("%s $$HOME/bin/nuget.exe install -source $$HOME/.nuget/packages -framework %s -excludeversion -o $$HOME/nuget/packages %s",
		  exelauncher, _OPTIONS["framework"] or "net471", wsp.name))
		os.execute(string.format("%s $$HOME/bin/nuget.exe search -source $$HOME/.nuget/packages %s",
		  exelauncher, wsp.name))

    os.copyfile("src/" .. wsp.name:lower() .. ".pc.in",
      string.format("%s/lib/pkgconfig/%s.pc",
      _OPTIONS["prefix"] or "/usr/local", wsp.name:lower()))
    os.execute(string.format("sh -xc \"pkg-config --with-path=%s/lib/pkgconfig --list-all | grep %s\"",
			_OPTIONS["prefix"] or "/usr/local", wsp.name:lower()))
	end
}

newaction { trigger = "package", description = "Package source archive(s)",
	execute = function ()
		local wsp = solution()
		distdir = string.format("%s-%s", wsp.name:lower(), wsp.version)
		os.copyfile(wsp.basedir .. "/exclude.lst", ".") ; os.mkdir(distdir)

		--[=[os.execute(string.format("cd %s ; zip -9 -q --exclude @%s/exclude.lst -r - ." ..
			" | unzip -od build/%s -", path.getdirectory(_WORKING_DIR),
			path.getdirectory(_WORKING_DIR), distdir))]=]
		os.execute(string.format("tar --format=posix --dereference " ..
			"--exclude-from=%s/exclude.lst -C %s -cf - . | tar -xpf - -C %s",
			path.getdirectory(_WORKING_DIR),
			path.getdirectory(_WORKING_DIR), distdir))

		for _, fmt in ipairs(string.explode(_OPTIONS["fmts"] or "tar.gz,zip", ",")) do
			if fmt == "7z" then
				os.execute(string.format("rm -f %s ; 7za a -t7z -mx=9 %s %s",
					distdir .. ".7z", distdir .. ".7z", distdir))
			elseif fmt == "zip" then
				os.execute(string.format("rm -f %s ; zip -9 -q -r %s %s",
					distdir .. ".zip", distdir .. ".zip", distdir))
			else
				os.execute(string.format("rm -f %s ; tar --posix -L -caf %s %s",
					distdir .. "." .. fmt, distdir .. "." .. fmt, distdir))
			end
		end
		os.rmdir(distdir)
	end
}

newaction { trigger = "monodoc", description = "Generate API documentation",
	execute = function ()
		os.execute(string.format("mdoc update -o doc_xmls -i %s.xml $$TARGET",
		  wsp.name))
		os.execute("mdoc export-html --force-update -o docs doc_xmls")
	end
}

newaction { trigger = "lint", description = "Lint check source code",
	execute = function ()
		local gendarmeapp = os.matchfiles("$$HOME/.nuget/packages/mono.gendarme/*/tools/gendarme.exe")[1]
		os.execute(string.format("%s %s --html lint_rpt.html $$TARGET",
			exelauncher, gendarmeapp))
	end
}

newaction { trigger = "monocover", description = "Report code coverage",
	execute = function ()
		local wsp = solution()
		os.execute(string.format("LD_LIBRARY_PATH=$$LD_LIBRARY_PATH:. MONO_PATH=$$MONO_PATH:. MONO_GAC_PREFIX=$$HOME/.local %s --debug -O=-aot --profile=coverage:output=cov.xml,covfilter-file=%s/resources/covfilter.txt --profile=log:output=build/cov.dat,covfilter-file=%s/resources/covfilter.txt %s %s %s.Tests.dll %s",
		  exelauncher, wsp.basedir, wsp.basedir, testconsoleapp, testappargs,
		  wsp.name, #_ARGS ~= 0 and table.concat(_ARGS, " ") or ""))
		os.execute("mprof-report --out=cov.txt cov.dat")
	end
}
