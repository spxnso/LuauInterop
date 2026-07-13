# Changelog

## [1.2.0](https://github.com/spxnso/LuauInterop/compare/v1.1.2...v1.2.0) (2026-07-13)


### Features

* add `.vscode` to gitignore ([f268393](https://github.com/spxnso/LuauInterop/commit/f2683937766f12c1464a6f53bfa9a534df888b5e))
* add `LuauInterop.Ast` ([222399a](https://github.com/spxnso/LuauInterop/commit/222399a6d39386b2de69c6c305c62ea8018e6d8b))
* add ast ([734d421](https://github.com/spxnso/LuauInterop/commit/734d4211f3529f89ba63370feda01d52e5b1924f))


### Bug Fixes

* rename `free` to `cpp_free` to avoid stack overflows ([241a211](https://github.com/spxnso/LuauInterop/commit/241a2110d06d3e669fce7efc9c68c43f94188e7f))
* update documentations files ([20a942f](https://github.com/spxnso/LuauInterop/commit/20a942f227b8dc71c57498813348dac868316d15))
* update vs version ([505d276](https://github.com/spxnso/LuauInterop/commit/505d27674a97475f01da1a25bf9efd17819ddbc4))

## [1.1.2](https://github.com/spxnso/LuauInterop/compare/v1.1.1...v1.1.2) (2026-06-01)


### Bug Fixes

* add None flag to LuauLibrary enum ([c266f69](https://github.com/spxnso/LuauInterop/commit/c266f6943a1eb8e05e7c24a8cad154e08c38f1f7))
* add sandbox tests and improve state tests for error handling ([ceb2d20](https://github.com/spxnso/LuauInterop/commit/ceb2d206f1b6919f7ba4720f00d0fc376ec440cd))
* change LuauDelegate class to sealed ([4cac929](https://github.com/spxnso/LuauInterop/commit/4cac9295444ef23888b0b95605c8139ad9401cd6))
* change LuauLibrary enum to use bitwise flags ([d9c4dfd](https://github.com/spxnso/LuauInterop/commit/d9c4dfd677fdecfcbdb1ec9e64c8083ccf46de11))
* clear pending exception before invoking managed function to ensure proper exception propagation ([c9be909](https://github.com/spxnso/LuauInterop/commit/c9be909829835bb340933e9b8f48845a250fe582))
* correct state reference usage in LuauBase class methods ([ce426d1](https://github.com/spxnso/LuauInterop/commit/ce426d12b1908b074370d756216037c383ba4724))
* correct thread state reference in sandbox thread isolation and remove useless `StripDebugInfo` ([ba69913](https://github.com/spxnso/LuauInterop/commit/ba6991363c8aae29cc694483e6aea9252d418132))
* forgot to remove debugging line ([a7ea90c](https://github.com/spxnso/LuauInterop/commit/a7ea90cfe5873db4b11546a35723bbc8db868b56))
* improve error handling by correctly reading byte length for error messages ([c2e9d4e](https://github.com/spxnso/LuauInterop/commit/c2e9d4e0c5fcbbe1098de62d5a54c28786c41e12))
* invalid stack behavior and C# callback error handling ([f7c4621](https://github.com/spxnso/LuauInterop/commit/f7c4621e226cf59d4ade3ca803cc41e1e596175b))
* make objects LuaState-aware to properly handle coroutines ([349d32d](https://github.com/spxnso/LuauInterop/commit/349d32d5d74bd8eb8afa0a33c9d3ea9cfcb25d10))
* properly implement default sandbox profile ([5372e4d](https://github.com/spxnso/LuauInterop/commit/5372e4d6c7f1f421e6ca2d2bf263bb5ca85f1120))
* Properly report errors to thread and other unintended side effects ([0b014ce](https://github.com/spxnso/LuauInterop/commit/0b014cea6c209fa70ddebb2c46ff8de767743fd2))
* refactor LuauSandboxOptions to use bitwise flag operations and rename DeniedGlobals to ForbiddenGlobals ([0b5e61a](https://github.com/spxnso/LuauInterop/commit/0b5e61a61337ccd378394f9eb57621af5c1eb821))
* refactor LuauThread to use CoroutineState directly and remove unnecessary GetCoroutine method ([e696c7a](https://github.com/spxnso/LuauInterop/commit/e696c7a603909e3c5cf33d7db8b03eedb40dda73))
* remove unnecessary region directives in LuaState class for cleaner code ([c586fc3](https://github.com/spxnso/LuauInterop/commit/c586fc3acb428d16ab83ff6d96e7f18307e0f506))
* remove unused parameter from Execute method in LuauSandbox class ([d64b660](https://github.com/spxnso/LuauInterop/commit/d64b660a276f9ca3b1915221f77b983285588bf5))
* replace OpenLibraries with OpenLibrary for specific library access in state and thread tests ([657c26e](https://github.com/spxnso/LuauInterop/commit/657c26e21f731ec22ece2bc6beae41e0261608ed))
* update callback tests for improved exception handling ([2fc5dbc](https://github.com/spxnso/LuauInterop/commit/2fc5dbcd7a9fd10618ccb9682466fbe59ce1c1c6))
* update coroutine state handling in LuauThread and Luau classes ([71511b4](https://github.com/spxnso/LuauInterop/commit/71511b490611466da3afd37ee1da635fca509f91))
* update exception handling to push error message to correct Lua state ([a2c5c44](https://github.com/spxnso/LuauInterop/commit/a2c5c446587047fc7a24068f8d6fbf0161ea9801))
* update LuauSandbox to correct state references ([1404dd3](https://github.com/spxnso/LuauInterop/commit/1404dd334df05c8bdc438b750811794fc7241571))

## [1.1.1](https://github.com/spxnso/LuauInterop/compare/v1.1.0...v1.1.1) (2026-05-30)


### Bug Fixes

* reset thread instead of closing state ([48ab89f](https://github.com/spxnso/LuauInterop/commit/48ab89fe263bdf18c1417663eec9dea8147e6541))

## [1.1.0](https://github.com/spxnso/LuauInterop/compare/v1.0.2...v1.1.0) (2026-05-30)


### Features

* add example for using `GetFFlag` ([65ad4e4](https://github.com/spxnso/LuauInterop/commit/65ad4e4d3b647c390cb3ef1ab7f4915a7b11201e))
* add GetFFlag method ([e40a921](https://github.com/spxnso/LuauInterop/commit/e40a9210f127031f95f2c590254619502690ca29))
* add LuauSandbox ([923de79](https://github.com/spxnso/LuauInterop/commit/923de7956dfcbb4170609f14e76cd08d6df3f35a))
* implement C# function callbacks ([99946de](https://github.com/spxnso/LuauInterop/commit/99946ded8caf2b6101986b0d09a9c25dde9408ea))

## [1.0.2](https://github.com/spxnso/LuauInterop/compare/v1.0.1...v1.0.2) (2026-05-29)


### Bug Fixes

* add README.md to package inclusion ([85adf4f](https://github.com/spxnso/LuauInterop/commit/85adf4fce33501d44792bb47f27284b437ded76c))
* change path to `docfx.json` and `_site` ([5e73f07](https://github.com/spxnso/LuauInterop/commit/5e73f07b3be495ede902f7dff92af332c0b4d3af))
* disable assembly info generation ([e0522b1](https://github.com/spxnso/LuauInterop/commit/e0522b14da35def99ad347151181444d12a2e8e9))

## [1.0.1](https://github.com/spxnso/LuauInterop/compare/v1.0.0...v1.0.1) (2026-05-28)


### Bug Fixes

* add `submodules` to publish job ([e1b48bd](https://github.com/spxnso/LuauInterop/commit/e1b48bdedec37e910e9e6074e000387833e2e456))

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
