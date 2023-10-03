ARG VARIANT=bullseye
ARG VERSION=3.10
FROM --platform=amd64 mcr.microsoft.com/devcontainers/python:${VERSION}-${VARIANT}

# Copy the setup scripts
COPY library-scripts/*.sh /tmp/library-scripts/

RUN export DEBIAN_FRONTEND=noninteractive \
    && apt-get update \
    && apt-get install -y xdg-utils \
    && curl -fsSL https://aka.ms/install-azd.sh | bash \
    && curl -fsSL https://pkgs.k8s.io/core:/stable:/v1.28/deb/Release.key | sudo gpg --dearmor -o /etc/apt/keyrings/kubernetes-apt-keyring.gpg \
    && apt-get update \
    && apt-get clean -y \
    && apt-get install -y kubectl \
    && rm -rf /var/lib/apt/lists/* \
    && curl -L https://github.com/kubernetes/kompose/releases/download/v1.31.0/kompose-linux-amd64 -o kompose \
    && chmod +x kompose
