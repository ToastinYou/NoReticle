# NoReticle
NoReticle removes the reticle when aiming a weapon.

# ACE Permissions
Apply these in your server's configuration file (`server.cfg`). Find more information about ACE Permissions [here](https://forum.cfx.re/t/ace-permissions/107706).

```
# Make the reticle visible for admins:
add_ace group.admin Reticle allow

# Make the reticle on the stun gun visible for everyone:
add_ace builtin.everyone ReticleStunGun allow
```

# Configurations (NEW!)
Apply these in your server's configuration file (`server.cfg`).
Set them to either `true` or `false`.

```
# Enable /weapons command for all players. This command gives the requesting player all available weapons with maximum ammunition:
noreticle_enable_give_all_weapons_command false

# Hide aircraft's reticle/HUD:
noreticle_hide_aircraft_reticle false
```
