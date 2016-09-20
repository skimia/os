FROM ubuntu:14.04

MAINTAINER Skimia Agency <contact@skimia.agency>
ENV DEBIAN_FRONTEND noninteractive

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

#5 Get Orchard from Github repository
RUN cd /home ; git clone https://github.com/skimia/os.git
RUN cd /home/os ; ./build.sh
RUN cd /home/os/src/SkimiaOS.ApiHost ; dotnet run
EXPOSE 5000
