# Changelog

## 1.0.0 (2026-05-28)


### Features

* add bindings for sandboxing functions ([eca1b33](https://github.com/spxnso/LuauInterop/commit/eca1b338e5ea7488aa93ee3e7d9f913ab4dae97f))
* add build instructions ([4cd00c1](https://github.com/spxnso/LuauInterop/commit/4cd00c18bdfe38c5ec2b0e6682c3b9ecee19239e))
* add build scripts for native library compilation ([2e83411](https://github.com/spxnso/LuauInterop/commit/2e83411fa53eb553ccf20b073a07b101fb466bf9))
* add CI and build-native workflows for automated testing and building ([1e5b67a](https://github.com/spxnso/LuauInterop/commit/1e5b67a133182752a2104d2fdaca1898c40bca73))
* add initial CMake configuration ([30c554f](https://github.com/spxnso/LuauInterop/commit/30c554f9118f27f407d487b6521de62d0b3b30f4))
* add intptr wrapper ([83f2618](https://github.com/spxnso/LuauInterop/commit/83f2618a5a1b1a34ce255c3b580e61d2a47a35f3))
* add linux-only support indication ([a0a61c6](https://github.com/spxnso/LuauInterop/commit/a0a61c6c97653e006a18ff8d8f185b47845a5bbb))
* add luau.def for export definitions and update CMake configuration ([3ea41c1](https://github.com/spxnso/LuauInterop/commit/3ea41c1e40888d04079dfd38faf347f8f1b4583b))
* add luau.dll for Windows x64 native runtime support ([9feab5c](https://github.com/spxnso/LuauInterop/commit/9feab5cd80462620aa2e73130569caf6131f8b47))
* implement IDisposable pattern across multiple classes and improve error handling ([8b63ef0](https://github.com/spxnso/LuauInterop/commit/8b63ef0ab43b672a2bd9b14938c8d67280d40333))
* **Luau:** add methods to open Lua libraries ([d524b37](https://github.com/spxnso/LuauInterop/commit/d524b37e163934b1f2a80c9fe51ee81cb18a8fa6))
* **Native:** implement native library loading across platforms ([3d6f9d8](https://github.com/spxnso/LuauInterop/commit/3d6f9d843d6b1be90ae39fa16332128c7de8ec0b))
* remove download native artifact step from CI workflow ([c847b31](https://github.com/spxnso/LuauInterop/commit/c847b31c74ed75017c1d0490690739349bbb88e8))
* replace platform-specific free function with lua_free for improved cross-platform compatibility ([ff70e30](https://github.com/spxnso/LuauInterop/commit/ff70e306f26e7786b95a2853b2f6874045adb48a))
* **tests:** add initial compilation tests for Luau compiler ([e0a9159](https://github.com/spxnso/LuauInterop/commit/e0a9159c1ca44f864686cd7b3a7d657198f521a9))
* update CI workflow to use Ubuntu 24.04 and add CMake installation step ([9db2c1d](https://github.com/spxnso/LuauInterop/commit/9db2c1dd9d4285baf2e2ef58d4a524b392be6e82))
* update greeting message in input script ([f82a27a](https://github.com/spxnso/LuauInterop/commit/f82a27a0a71b666fbc3521b2946f63dca8aeff7c))
* update input script and refactor execution to use chunk compilation ([51fd6c1](https://github.com/spxnso/LuauInterop/commit/51fd6c1885bc9a3950e8a3b92125b450ada39829))
* update libluau.so for `lua_free` ([94a9e7f](https://github.com/spxnso/LuauInterop/commit/94a9e7f8f84a24e6f0cd1c7c3c17c84051b2d44b))


### Bug Fixes

* add inner exception handling for more clarity ([6a4836d](https://github.com/spxnso/LuauInterop/commit/6a4836d5c386b9163664846146287991a146e83b))
* change branch to master ([6c7a7c5](https://github.com/spxnso/LuauInterop/commit/6c7a7c51aabee729f72abbd7d1ec322513a28e67))
* change namespace ([fcd8420](https://github.com/spxnso/LuauInterop/commit/fcd84203b64f49364958800846ec61589a50482e))
* change path of luau runtime ([c2b7d59](https://github.com/spxnso/LuauInterop/commit/c2b7d59452d8b7f8a30e67144de6e1a161be5831))
* comment out unused lua_isinteger64 method due to missing implementation ([a421c82](https://github.com/spxnso/LuauInterop/commit/a421c8251b7b60268f4db357b34124f584ff9817))
* **compiler:** improve error message for compilation failure ([24e646f](https://github.com/spxnso/LuauInterop/commit/24e646f5ab82a27e3b778b8521114c8bd83597b3))
* **Luau:** implement IDisposable pattern and improve resource management ([166473a](https://github.com/spxnso/LuauInterop/commit/166473ac123d3b023012de160b7ea0acdf515f43))
* **Native:** remove StringMarshalling from LibraryImport attributes for consistency ([dc30779](https://github.com/spxnso/LuauInterop/commit/dc307792d83c93d62318e6f731a778ad1d52df2c))
* remove unused glue.c file creation from build scripts and update .gitignore ([1c0b446](https://github.com/spxnso/LuauInterop/commit/1c0b446a45c1e1504d52148928cd31f2d0f2974a))
* rename variable for Luau instance for clarity ([b3bdd6a](https://github.com/spxnso/LuauInterop/commit/b3bdd6ac53809fc01dc007ab10491bbad1a4959b))
* reorder project file elements for consistency ([fc6bf9b](https://github.com/spxnso/LuauInterop/commit/fc6bf9b196aae563b9b0c09ffd1e4725b6b27fa1))
* update build-native.bat for improved directory handling and CMake configuration ([c9feebf](https://github.com/spxnso/LuauInterop/commit/c9feebf54f31548baa697b927eff06447dd132eb))
* update project name to LuauInterop in README ([2fd2992](https://github.com/spxnso/LuauInterop/commit/2fd2992913bdd4af1b21dc75f37656617eca8f90))
