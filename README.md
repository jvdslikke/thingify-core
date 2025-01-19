# thingify-core

Easy-to-use home automation software with containerized plugins.

# The idea

I found Home Assistant quite difficult to use. 

Also a new plugin was hard to get included because a pull request had to be accepted, while they have a lot of open pull requests. If plugins could run in containers, they can be secured so no need to have them reviewed. 

# Roadmap
 
1. Main service + apps to read and show sensor data
2. App store
3. Storage
4. Authentication
5. Automation 

# Architecture

Main service in docker.

Apps/plugins running in docker. Only permission to access what they need. 

```mermaid
block-beta
    block:os
        columns 2
        ost("OS (Fedora CoreOS?)"):2
        Cockpit
        block usr 
            usrt("User")
            columns 1
            block:podman
                columns 1
                podmant("Podman")
                thingc("Thingify-core")
            en
        end
    end

class ost BT
class usert BT
class podmant BT
classDef BT stroke:transparent,fill:transparent
```

## PoC docker network setup (in progress)

Run container for main service:

podman run -it --rm --name thingify-core alpine sh

TODO add to network. 
