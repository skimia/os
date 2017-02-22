FROM debian:jessie


MAINTAINER Skimia Agency <contact@skimia.agency>

#0 fix

ENV DEBIAN_FRONTEND noninteractive
ENV KOREBUILD_SKIP_RUNTIME_INSTALL true

RUN echo 'debconf debconf/frontend select Noninteractive' | debconf-set-selections


#1 Add the new apt-get feed

RUN sh -c 'echo "deb [arch=amd64] http://apt-mo.trafficmanager.net/repos/dotnet/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list'
RUN apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893

#2 Update and install basic packages needed

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
# .NET Core dependencies
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu52 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libunwind8 \
        libuuid1 \
        zlib1g \
        gettext \
        zip \
        unzip \       
        psmisc \      
        uuid-runtime \
        git \
#3 Install .NET Core
        dotnet-dev-1.0.0-preview2-1-003177 \
#4 Install Mono (Required by KoreBuild)
        mono-devel

#5 Install libuv (Required by Kestrel)

RUN apt-get -qqy install \
    curl \
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

#6 Clean

RUN rm -rf /var/lib/apt/lists/*

#7 Get OS from local dir

COPY . /home/os
RUN cd /home/os ; chmod +x ./build.sh && sleep 1 && ./build.sh repo-initialize

#RUN cd /home/os/git/ ; git submodule init ; git submodule update

#8 Build OS
RUN cd /home/os/src/SkimiaOS.ApiHost ; dotnet publish

#9 Run OS

EXPOSE 80
WORKDIR /home/os/src/SkimiaOS.ApiHost/bin/Debug/netcoreapp1.1/publish/
CMD ["/usr/bin/dotnet SkimiaOS.ApiHost.dll"]
