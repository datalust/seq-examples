Seq Examples
===================

A collection of examples demonstrating how to do things with Seq.

`/clients` - contains examples of different ways to connect a client to Seq

`/cluster` - contains examples of configuring Seq into a DR cluster

A suitable temporary Seq server can be started with:

```shell
> docker run -e ACCEPT_EULA=Y --rm -p 5341:80 datalust/seq
```