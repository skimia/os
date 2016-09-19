# os
Skimia servier operating system for create distribued web apps


## Getting started

### Using Docker

#### compile
```
wget https://raw.githubusercontent.com/skimia/os/master/Dockerfile
docker build -t skimia/os .
docker run -ti -p 80:8080 skimia/os
```
#### Docker Hub

[![Dockerfile](https://img.shields.io/badge/Hub-Dockerfile-blue.svg?style=flat-square)](https://hub.docker.com/r/skimia/os/)

```
docker run -ti -p 80:8080 skimia/os
```
