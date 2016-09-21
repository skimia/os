FROM ubuntu:14.04

MAINTAINER Skimia Agency <contact@skimia.agency>

#0 fix

ENV DEBIAN_FRONTEND noninteractive
RUN echo 'debconf debconf/frontend select Noninteractive' | debconf-set-selections
ENV KOREBUILD_SKIP_RUNTIME_INSTALL true

#1 Update and install basic packages needed

RUN sudo apt-get update
RUN sudo apt-get install -y gettext zip unzip git uuid-runtime

#2 Add the new apt-get feed

RUN sudo sh -c 'echo "deb [arch=amd64] http://apt-mo.trafficmanager.net/repos/dotnet/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list'
RUN sudo apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
RUN sudo apt-get update

#3 Install .NET Core

RUN sudo apt-get install -y dotnet-dev-1.0.0-preview2-003131

#4 Install Mono (Required by KoreBuild)

RUN sudo apt-get install -y mono-devel

#5 Get OS from local dir
COPY . /home/os
RUN cd /home/os ; chmod +x ./build.sh ; ./build.sh

EXPOSE 5000

#6 Run OS

CMD cd /home/os/src/SkimiaOS.ApiHost ; dotnet run
