docker build --file Dockerfile.buildPublish --label buildPublishResult-dev --progress=plain --no-cache .;
docker system prune --all --volumes --force;