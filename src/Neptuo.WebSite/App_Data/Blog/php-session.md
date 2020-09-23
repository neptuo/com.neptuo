This is quick tip, out of my tipical blog themes, intended for my future me. Please, make me read this next time I need to make the PHP session last longer.

> I'm going to show a way to do this through PHP ini/settings. All of this stuff can also be accomplished through PHP code.

The first issue is that by default PHP session cookie has lifetime of session. So when you close the browser, the cookie is gone.

```
session.cookie_lifetime = 86400
```

When to set the `session.cookie_lifetime` to 0, it means a session lifetime; all other values sets the expiration in secconds, so 86400s = 24h.

The second issue is server-side and is about throwing away session data as garbage.

```
session.gc_maxlifetime = 86400
```

The `session.gc_maxlifetime` specifies the number of seconds after which data will be seen as 'garbage' and potentially cleaned up. The default value is 1440s = 24min. 

As a bonus, you can easily rename the cookie. By default it is called `PHPSESSID`, but you can rename it with `session.name`.

## Where to set these?

You can place them directly in your `php.ini`, but on web hosting you won't have the access. Fortunately most hostings support user defined ini, the one I'm using - onebit.cz - has a convention with file named `.user.ini`.

## The result

Here is my `.user.ini`

```
session.name = is4wfw
session.gc_maxlifetime = 86400
session.cookie_lifetime = 86400
```