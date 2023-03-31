# JupeBot

An IRC bot which allows you to easily introduce additional clients to the network.

This bot is only intended for testing other bots on test networks. It has no access control and requires oper privileges to run a CHGHOST to easily identify the fake clients.

## IRCd config

It's strongly recommended that the connection class has either a very high or no clones limit, as this will limit the number of additional clients the bot can introduce.

For Solanum, something like this works:
```
auth {
    spoof = "jupiter/.";
    flags = no_tilde, exceed_limit;
    user = "~jupe@192.168.22.*";
    class = "users";
};
```

You'll also need to grant privs; for Solanum, this works:
```
privset "jupebot" {
    privs = oper:general, oper:hidden_admin;
};

operator "jupebot" {
    user = "*@*";
    password = "changeme";
    flags = ~encrypted, need_ssl;
    privset = "jupebot";
};
```

Finally, you'll need to make sure the m_chghost module is loaded, and the `--enable-oper-chghost` flag was set at build time.
