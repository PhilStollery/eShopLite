ARG VARIANT=bullseye
ARG VERSION=3.10
FROM --platform=amd64 mcr.microsoft.com/devcontainers/python:${VERSION}-${VARIANT}

# Copy the setup scripts
COPY library-scripts/*.sh /tmp/library-scripts/

RUN export DEBIAN_FRONTEND=noninteractive \
    && curl -fsSL https://pkgs.k8s.io/core:/stable:/v1.28/deb/Release.key | gpg --dearmor -o /etc/apt/keyrings/kubernetes-apt-keyring.gpg \
    && apt-get update \
    && apt-get install -y xdg-utils \
    && apt-get install -y kubectl \    
    && curl -fsSL https://aka.ms/install-azd.sh | bash \
    && apt-get clean -y \
    && rm -rf /var/lib/apt/lists/* 
