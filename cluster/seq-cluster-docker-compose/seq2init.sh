#!/bin/bash

# pre-generated with `docker run --rm -e ACCEPT_EULA=Y datalust/seq:preview show-key --generate`
KEY=vg0O5h5/JQcwMEh/BYQ+plB/xtX3Okj/5KNRrhegPSI=

seqsvr config create
seqsvr config set -k storage.secretKey -v "$KEY"

# seqsvr secret set -k metastore.postgres.connectionString -v "Host=pgseq;Port=5433;Database=seq;Username=postgres;Password=terminatorx"

# this is the outside address of the load balancer (i.e. the port exposed by the `pgseq` service)
# seqsvr config set -k api.canonicalUri -v "http://localhost:5666"
