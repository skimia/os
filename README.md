# os
Skimia servier operating system for create distribued web apps

[![Get My Slack Invite](https://slackin-skimiaos.herokuapp.com/badge.svg)](https://slackin-skimiaos.herokuapp.com/)

[![license](https://img.shields.io/github/license/skimia/os.svg?maxAge=2592000&style=flat-square)](https://github.com/skimia/os/blob/master/LICENSE)
[![Maintenance](https://img.shields.io/maintenance/yes/2017.svg?maxAge=2592000&style=flat-square)]()

## Getting started

### Using Docker

#### compile
```
git clone git@github.com:skimia/os.git
cd os
docker build -t skimia/os .
docker run -ti -p 80 skimia/os
```
#### Docker Hub

[![Dockerfile](https://img.shields.io/badge/Hub-Dockerfile-blue.svg?style=flat-square)](https://hub.docker.com/r/skimia/os/)

```
docker run -ti -p 80 skimia/os
```

## Build Status

| Build server| Platform       | Status (master) | Status (dev)                                                                                                                                                               | Coverage (master) | Coverage (dev) |
|-------------|----------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----|----|
| AppVeyor    | Windows: .Net Core       |  [![Build status](https://img.shields.io/appveyor/ci/kesslerdev/os/master.svg?style=flat-square)](https://ci.appveyor.com/project/kesslerdev/os) |  [![Build status](https://img.shields.io/appveyor/ci/kesslerdev/os/dev.svg?style=flat-square)](https://ci.appveyor.com/project/kesslerdev/os/history) | [![Coveralls master](https://img.shields.io/coveralls/skimia/os/master.svg?style=flat-square)](https://coveralls.io/github/skimia/os?branch=master) | [![Coveralls dev](https://img.shields.io/coveralls/skimia/os/dev.svg?style=flat-square)](https://coveralls.io/github/skimia/os?branch=dev) |
| Travis      | Linux & OSX: .Net Core   |[![Build Status](https://img.shields.io/travis/skimia/os/master.svg?style=flat-square)](https://travis-ci.org/skimia/os)                                                 |[![Build Status](https://img.shields.io/travis/skimia/os/dev.svg?style=flat-square)](https://travis-ci.org/skimia/os/branches)                                                 | x | x |

## Developpement Status

| Status         | Badge                                                                                                                                              | Description                                                          |
|----------------|----------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------|
| Backlog        | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/backlog.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)            | All project issues not scheduled.                                    |
| Ready          | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/ready.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)              | Defined issues, will probably be part of current in next iterations. |
| Current        | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/current.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)            | Issues which are part of current iteration.                          |
| In Progress    | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/in%20progress.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)      | Issue currently treated.                                             |
| Packaging      | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/packaging.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os)          | Issue fixed, wait to be merged in DEV.                               |
| Ready To Merge | [![Waffle.io](https://img.shields.io/waffle/label/skimia/os/ready%20to%20merge.svg?maxAge=2592000&style=flat-square)](https://waffle.io/skimia/os) | Wait to be merged in master and released in new version              |

## Contributing

We currently follow the these [contributing guidelines.](https://github.com/skimia/os/blob/master/CONTRIBUTING.md)

## Configuration

### Global Options (Main Application)

#### Hosting Options

##### Urls

define the URLs Kestrel would bind to **default:** `http://localhost:5000`

#### ExtCore Options

##### Extensions

define the extensions directory used by ExtCore

```json
{
  "Host":{
    "Urls":["http://::80", "http://0.0.0.0:80"]
  },
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "Extensions": {
    // Please keep in mind that you have to change '\' to '/' on Linux-based systems
    "Path": "\\Plugins"
  },
  "Data": {
    "DefaultConnection": {
      // Please keep in mind that you have to change '\' to '/' on Linux-based systems
      "ConnectionString": "Data Source=..\\..\\..\\db.sqlite"
    }
  },
  "Plugins": {
    "StaticFiles": {
      "Path": "apps",
      "AutomaticResolution":  false,
      "Applications": [
        {
          "Name": "os.web",
          "Path": "apps\\os.web",
          "BindTo": "/",
          "Rewrite": {
            "Apache": "rewrite.apache"
          }
        }
      ]
    }
  }
}

```
