#!/bin/bash
set -e

if [[ "$OSTYPE" == "darwin"* ]]; then
    PLATFORM="osx-x64"
    LIB_NAME="libluau.dylib"
    JOBS=$(sysctl -n hw.logicalcpu)
else
    PLATFORM="linux-x64"
    LIB_NAME="libluau.so"
    JOBS=$(nproc)
fi

# Build
rm -rf build
mkdir build && cd build
cmake .. -DCMAKE_BUILD_TYPE=Release
cmake --build . --config Release -- -j$JOBS

# Copy to native
mkdir -p ../src/Luau.Native/runtimes/$PLATFORM/native
cp $LIB_NAME ../src/Luau.Native/runtimes/$PLATFORM/native/$LIB_NAME

echo "Done! $LIB_NAME copied to src/Luau.Native/runtimes/$PLATFORM/native/"

# Cleanup
cd ..
rm -rf build