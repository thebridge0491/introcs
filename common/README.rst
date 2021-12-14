Introcs.Util
===========================================
.. .rst to .html: rst2html5 foo.rst > foo.html
..                pandoc -s -f rst -t html5 -o foo.html foo.rst

Utilities sub-package for CSharp Intro examples project.

Installation
------------
source code tarball download:
    
        # [aria2c --check-certificate=false | wget --no-check-certificate | curl -kOL]
        
        FETCHCMD='aria2c --check-certificate=false'
        
        $FETCHCMD https://bitbucket.org/thebridge0491/introcs/[get | archive]/master.zip

version control repository clone:
        
        git clone https://bitbucket.org/thebridge0491/introcs.git

build example with make:
[sh] ./configure.sh [--prefix=$PREFIX] [--help]

make restore

make all [testcompile check]

make nugetadd [nugetinstall]

build example with meson:
[env PKG_CONFIG_PATH=$PREFIX/lib/pkgconfig] meson setup -Dprefix=$PREFIX -Dhome=$HOME build

meson compile -C build restore

meson compile -C build ; meson compile -C build check

meson compile -C build nugetadd [nugetinstall]

build example with msbuild:
msbuild /t:restore [/t:restore tests/*.*proj]

[LD_LIBRARY_PATH=$PREFIX/lib] msbuild /t:build [/t:build,test tests/*.*proj]

msbuild /t:nugetadd,nugetinstall

Usage
-----
        // PKG_CONFIG='pkg-config --with-path=$PREFIX/lib/pkgconfig'
        
        // $PKG_CONFIG --cflags --libs <ffi-lib>

        using Introcs.Util;
        
        ...
        
        int[] arr1 = {0, 1, 2}, arr2 = {10, 20, 30};
        
        int[][] nested_arr = CartesianProd(arr1, arr2);

Author/Copyright
----------------
Copyright (c) 2021 by thebridge0491 <thebridge0491-codelab@yahoo.com>

License
-------
Licensed under the Apache-2.0 License. See LICENSE for details.
