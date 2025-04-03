# Changelog

## [v7.0.0] - 2024-12-21
### :boom: BREAKING CHANGES
- due to [`6059b1b`](https://github.com/sandre58/MyNetObservable/commit/6059b1b27682b143a60e183ada4e3953aae2b19d) - Add .NET 9.0 support, update packages, and fix attributes:

  Add .NET 9.0 support


### :sparkles: New Features
- [`8fcf153`](https://github.com/sandre58/MyNetObservable/commit/8fcf15366f2249b3b6bf25bede69c235bf7ceeaf) - add SingleTaskDeferrer
- [`9de28b9`](https://github.com/sandre58/MyNetObservable/commit/9de28b9f9ab7578267f650a72e07df9b70026f7e) - add Action Runner
- [`6059b1b`](https://github.com/sandre58/MyNetObservable/commit/6059b1b27682b143a60e183ada4e3953aae2b19d) - Add .NET 9.0 support, update packages, and fix attributes

### :bug: Bug Fixes
- [`aea2756`](https://github.com/sandre58/MyNetObservable/commit/aea27569f9c0942ecca990109e2eab046b080aad) - remove refresh sort for xtendedCollection
- [`9d77433`](https://github.com/sandre58/MyNetObservable/commit/9d7743341bede91cda6cd129ecb9209ed8635020) - remove old item from cache in collection

### :recycle: Refactors
- [`96c0937`](https://github.com/sandre58/MyNetObservable/commit/96c093774356680fac49df82760e2dd5db7194e4) - Optimize collections and update package references


## [v5.2.0] - 2024-07-21
### :wrench: Chores
- [`6b0f5e6`](https://github.com/sandre58/MyNetObservable/commit/6b0f5e656974f81394a7541c824dedb6cb31537f) - update packages *(commit by [@sandre58](https://github.com/sandre58))*


## [v5.1.0] - 2024-07-17
### :sparkles: New Features
- [`993a2ac`](https://github.com/sandre58/MyNetObservable/commit/993a2ac8bf8397ad01da9fa40ff5786b824892a3) - add SetIsModified() *(commit by [@sandre58](https://github.com/sandre58))*


## [v5.0.1] - 2024-06-20
### :bug: Bug Fixes
- [`ae7a49c`](https://github.com/sandre58/MyNetObservable/commit/ae7a49c2fb4903233d39b51e92251ca4133cbbc7) - fix key in cache in RefreshDeferrer *(commit by [@sandre58](https://github.com/sandre58))*


## [v5.0.0] - 2024-06-20
### :boom: BREAKING CHANGES
- due to [`2e6829a`](https://github.com/sandre58/MyNetObservable/commit/2e6829a753c4a7a5a9b7e7777bf09c468f723b59) - add suspend and unsubscribe features in RefreshDeferrer *(commit by [@sandre58](https://github.com/sandre58))*:

  `Subscribe` method take subscriber in first parameter


### :sparkles: New Features
- [`2e6829a`](https://github.com/sandre58/MyNetObservable/commit/2e6829a753c4a7a5a9b7e7777bf09c468f723b59) - add suspend and unsubscribe features in RefreshDeferrer *(commit by [@sandre58](https://github.com/sandre58))*


## [v4.0.0] - 2024-06-13
### :boom: BREAKING CHANGES
- due to [`6853f1a`](https://github.com/sandre58/MyNetObservable/commit/6853f1a4c4877a3cac47bbbda00afd0adf70c7cf) - remove collections *(commit by [@sandre58](https://github.com/sandre58))*:

  remove collections


### :sparkles: New Features
- [`6853f1a`](https://github.com/sandre58/MyNetObservable/commit/6853f1a4c4877a3cac47bbbda00afd0adf70c7cf) - remove collections *(commit by [@sandre58](https://github.com/sandre58))*


## [v3.2.0] - 2024-06-13
### :sparkles: New Features
- [`c97d34b`](https://github.com/sandre58/MyNetObservable/commit/c97d34b744485b0234818e87c8eaa0ab3c0dc48f) - add refreshDeferrer *(commit by [@sandre58](https://github.com/sandre58))*


## [v3.1.0] - 2024-06-06
### :sparkles: New Features
- [`5c2cdc7`](https://github.com/sandre58/MyNetObservable/commit/5c2cdc7c55a159c140abea8de4332c59add6b015) - Make IsModified method "virtual" *(commit by [@sandre58](https://github.com/sandre58))*


## [v3.0.0] - 2024-05-25
### :boom: BREAKING CHANGES
- due to [`7c062f8`](https://github.com/sandre58/MyNetObservable/commit/7c062f8ffc442870056bb04a057c138dfdfe425d) - Improve SourceProviders *(PR [#1](https://github.com/sandre58/MyNetObservable/pull/1) by [@sandre58](https://github.com/sandre58))*:

  Improve SourceProviders (#1)


### :sparkles: New Features
- [`7c062f8`](https://github.com/sandre58/MyNetObservable/commit/7c062f8ffc442870056bb04a057c138dfdfe425d) - Improve SourceProviders *(PR [#1](https://github.com/sandre58/MyNetObservable/pull/1) by [@sandre58](https://github.com/sandre58))*


## [v2.0.0] - 2024-05-23
### :boom: BREAKING CHANGES
- due to [`2cb42f0`](https://github.com/sandre58/MyNetObservable/commit/2cb42f094e931d04ec836398185ff26e8adf5a1e) - change name of ISourceProvider implementations *(commit by [@sandre58](https://github.com/sandre58))*:

  change name of ISourceProvider implementations


### :recycle: Refactors
- [`2cb42f0`](https://github.com/sandre58/MyNetObservable/commit/2cb42f094e931d04ec836398185ff26e8adf5a1e) - change name of ISourceProvider implementations *(commit by [@sandre58](https://github.com/sandre58))*


## [v1.2.0] - 2024-05-14
### :bug: Bug Fixes
- [`4035f1f`](https://github.com/sandre58/MyNetObservable/commit/4035f1f76f61f2da7680466d17c9dcda5c6bc97f) - add Fody dependencies *(commit by [@sandre58](https://github.com/sandre58))*

### :wrench: Chores
- [`9668222`](https://github.com/sandre58/MyNetObservable/commit/9668222c1e6dbedd01da026dd7950e1ec9f8807c) - upgrade MyNet.Utilities dependency to version 2.0.0 *(commit by [@sandre58](https://github.com/sandre58))*


## [v1.1.0] - 2024-05-02
### :recycle: Build
- [`e175c21`](https://github.com/sandre58/MyNetObservable/commit/e175c21ce55ff3ab134608dcbc984e37517f098c) - bump MyNet.DynamicData.Extensions version to "2.0.0" *(commit by [@sandre58](https://github.com/sandre58))*:


## [v1.0.1] - 2024-04-27
- Initialize repository
[v1.2.0]: https://github.com/sandre58/MyNetObservable/compare/v1.1.0...v1.2.0
[v2.0.0]: https://github.com/sandre58/MyNetObservable/compare/v1.2.0...v2.0.0
[v3.0.0]: https://github.com/sandre58/MyNetObservable/compare/v2.0.0...v3.0.0
[v3.1.0]: https://github.com/sandre58/MyNetObservable/compare/v3.0.0...v3.1.0
[v3.2.0]: https://github.com/sandre58/MyNetObservable/compare/v3.1.0...v3.2.0
[v3.3.0]: https://github.com/sandre58/MyNetObservable/compare/v3.2.0...v3.3.0
[v4.0.0]: https://github.com/sandre58/MyNetObservable/compare/v3.2.0...v4.0.0
[v5.0.0]: https://github.com/sandre58/MyNetObservable/compare/v4.0.0...v5.0.0
[v5.0.1]: https://github.com/sandre58/MyNetObservable/compare/v5.0.0...v5.0.1
[v5.1.0]: https://github.com/sandre58/MyNetObservable/compare/v5.0.1...v5.1.0
[v5.1.0]: https://github.com/sandre58/MyNetObservable/compare/v5.0.1...v5.1.0
[v5.2.0]: https://github.com/sandre58/MyNetObservable/compare/v5.1.0...v5.2.0
[v7.0.0]: https://github.com/sandre58/MyNetObservable/compare/v6.0.0...v7.0.0
[v8.0.0]: https://github.com/sandre58/MyNetObservable/compare/v7.0.0...v8.0.0