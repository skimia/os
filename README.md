# os
Skimia servier operating system for create distribued web apps

[![Slack](https://img.shields.io/badge/Slack-Team-green.svg?maxAge=2592000&style=flat-square)](https://skimiaos.slack.com)

[![license](https://img.shields.io/github/license/skimia/os.svg?maxAge=2592000&style=flat-square)](https://github.com/skimia/os/blob/master/LICENSE)
[![Maintenance](https://img.shields.io/maintenance/yes/2016.svg?maxAge=2592000&style=flat-square)]()

## Getting started

### Using Docker

#### compile
```
git clone git@github.com:skimia/os.git
cd os
docker build -t skimia/os .
docker run -ti -p 80:5000 skimia/os
```
#### Docker Hub

[![Dockerfile](https://img.shields.io/badge/Hub-Dockerfile-blue.svg?style=flat-square)](https://hub.docker.com/r/skimia/os/)

```
docker run -ti -p 80:5000 skimia/os
```

## Build Status

| Build server| Platform       | Status (master) | Status (dev)                                                                                                                                                               |
|-------------|----------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AppVeyor    | Windows: .Net Core       |  [![Build status](https://img.shields.io/appveyor/ci/kesslerdev/os/master.svg?style=flat-square)](https://ci.appveyor.com/project/kesslerdev/os) |  [![Build status](https://img.shields.io/appveyor/ci/kesslerdev/os/dev.svg?style=flat-square)](https://ci.appveyor.com/project/kesslerdev/os/history) |
| Travis      | Linux & OSX: .Net Core   |[![Build Status](https://img.shields.io/travis/skimia/os/master.svg?style=flat-square)](https://travis-ci.org/skimia/os)                                                 |[![Build Status](https://img.shields.io/travis/skimia/os/dev.svg?style=flat-square)](https://travis-ci.org/skimia/os/branches)                                                 |

## Developpement Status

| Status         | Badge                                                                                                                                              | Description                                                          |
|----------------|----------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------|
| Backlog        | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/backlog.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)            | All project issues not scheduled.                                    |
| Ready          | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/ready.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)              | Defined issues, will probably be part of current in next iterations. |
| Current        | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/current.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)            | Issues which are part of current iteration.                          |
| In Progress    | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/in%20progress.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)      | Issue currently treated.                                             |
| Packaging      | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/packaging.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)          | Issue fixed, wait to be merged in DEV.                               |
| Ready To Merge | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/ready%20to%20merge.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os) | Wait to be merged in master and released in new version              |
