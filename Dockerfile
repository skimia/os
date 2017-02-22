FROM ubuntu:14.04

MAINTAINER Skimia Agency <contact@skimia.agency>

#0 fix

ENV DEBIAN_FRONTEND noninteractive
ENV KOREBUILD_SKIP_RUNTIME_INSTALL true

RUN echo 'debconf debconf/frontend select Noninteractive' | debconf-set-selections

#1 Update and install basic packages needed

RUN sudo apt-get update
RUN sudo apt-get install -y gettext zip unzip git uuid-runtime psmisc

#2 Add the new apt-get feed

RUN sudo sh -c 'echo "deb [arch=amd64] http://apt-mo.trafficmanager.net/repos/dotnet/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list'
RUN sudo apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
RUN sudo apt-get update

#3 Install .NET Core

RUN sudo apt-get install -y dotnet-dev-1.0.0-preview2-1-003177

#4 Install Mono & node (Required by KoreBuild)

RUN sudo apt-get install -y mono-devel
RUN sudo apt-get install -y nodejs npm && sudo ln -s /usr/bin/nodejs /usr/local/bin/node && sudo ln -s /usr/bin/npm /usr/local/bin/npm


#4 Install libuv (Required by Kestrel)
RUN apt-get -qqy install \
	autoconf \
	automake \
	build-essential \
	libtool
RUN LIBUV_VERSION=1.4.2 \
	&& curl -sSL https://github.com/libuv/libuv/archive/v${LIBUV_VERSION}.tar.gz | tar zxfv - -C /usr/local/src \
	&& cd /usr/local/src/libuv-$LIBUV_VERSION \
	&& sh autogen.sh && ./configure && make && make install \
	&& rm -rf /usr/local/src/libuv-$LIBUV_VERSION \
	&& ldconfig



#6 Get OS from local dir
COPY . /home/os
RUN cd /home/os ; chmod +x ./build.sh && sleep 1 && ./build.sh repo-initialize

#RUN cd /home/os/git/ ; git submodule init ; git submodule update

#7 Build OS
RUN cd /home/os/src/SkimiaOS.ApiHost ; dotnet publish

#8 Run OS

EXPOSE 80
WORKDIR /home/os/src/SkimiaOS.ApiHost/bin/Debug/netcoreapp1.0/publish/
CMD ["sudo /usr/bin/dotnet SkimiaOS.ApiHost.dll"]
