Seq DR Cluster Docker Demo
==========================

A docker compose demo that creates a Seq high availability cluster.

> This is an example only. It is not secure. Don't use in production. 

Reading
------

* https://docs.datalust.co/docs/init-scripts
* https://docs.datalust.co/docs/using-postgresql-as-a-metastore
* https://docs.nginx.com/nginx/admin-guide/load-balancer/http-load-balancer/

Usage
-----

1. Have docker compose
1. Purchase [Seq Datacenter](https://datalust.co/pricing) or [start a trial](https://datalust.co/trial)
1. Copy license certificate into a file called `cert.txt`
1. `docker compose up`
1. Browse [`http://localhost:5666`](http://localhost:5666) for Seq, [`http://localhost:5777`](http://localhost:5777) for the Seq diagnostic instance and [`http://localhost:8404/stats`](http://localhost:8404/stats) for the HAProxy dashboard.

How it works
------------

The `compose.yaml` defines a docker network with:
* a Postgresql server
* two Seq servers in a cluster
* an haproxy server
* a diagnostic Seq instance that receives telemetry from the cluster node
* a number of other containers for running various `seqcli` and `curl` commands

HAProxy is configured (`haproxy/haproxy.cfg`) as a round robin reverse proxy listening on `5666`.

The diagnostic instance is exposed on `5777`.
