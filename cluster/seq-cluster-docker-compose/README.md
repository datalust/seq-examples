Seq DR Cluster Docker Demo
==========================

A docker compose demo that creates a Seq DR cluster.

> This is an example only. It is not secure. Don't use in production. 

Reading
------

* https://docs.datalust.co/docs/init-scripts
* https://docs.datalust.co/docs/using-postgresql-as-a-metastore
* https://docs.datalust.co/docs/setting-up-a-disaster-recovery-dr-instance
* https://docs.nginx.com/nginx/admin-guide/load-balancer/http-load-balancer/

Usage
-----

1. Have docker compose
1. Purchase [Seq Datacenter](https://datalust.co/pricing) or [start a trial](https://datalust.co/trial)
1. Copy license certificate into a file called `cert.txt`
1. `docker compose up`
1. Browse [`http://localhost:5666`](http://localhost:5666)

How it works
------------

The `compose.yaml` defines a docker network with a Postgresql server, two Seq servers and an nginx server. The scripts `seq1init.sh` and `seq2init.sh` are mapped into their respective Seq containers and configure the leader (seq1) and the follower (seq2). Once `seq1` and `seq2` are healthy `seq1setlicense` and `seq2setlicense` use `seqcli` to apply the license certificate (`cert.txt`) to their respective servers. Nginx is configured as a round robin reverse proxy listening on `5666`, which is the only port exposed from the docker network. 