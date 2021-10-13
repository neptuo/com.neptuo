I'm working a lot with PHP in recent months. As I didn't want to install all the tools on my computer, I went docker the way. My most recent PHP project is a module for is4wfw.

> is4wfw is a CMS written in PHP that supports extensibility through modules from the 341.0 version.

So, for development of this module, I created a docker-compose to spin-up all the services it requires.

```yaml
services:
  web:
    image: neptuo/is4wfw:341.1
    links:
      - mysql:db
    ...

  phpmyadmin:
    image: phpmyadmin/phpmyadmin
    links:
      - mysql:db
    ...
        
  mysql:
    image: mysql:8.0.19
    ...
```

After a bit of time I needed to install some dependency using the composer. So I did that manually and was happy.

Next week I wanted to continue on a different computer, I pulled the repository typed `docker-compose up`, but the dependency wasn't there. Then I realized that I can ensure installation of the composer dependency as a separate container in the compose.

> It is similar pattern that is used in Kubernetes and it is called a 'sidecar container'

```yaml
...
  composer:
    image: composer
    volumes:
      - ./:/app
    command: [ "composer", "install" ]

  web:
    image: neptuo/is4wfw:341.1
    depends_on:
      - "composer"
...
```

This setup ensures that the composer container starts before the web container, install dependencies and that happily exists.

And that's it.