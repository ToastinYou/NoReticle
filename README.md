# NoReticle
NoReticle removes the reticle when aiming a weapon.

# Ace Permissions
Apply these in your server's configuration file (server.cfg).

Make the reticle visible for admins:
add_ace group.admin Reticle allow

Make the reticle on the stun gun visible for everyone:
add_ace builtin.everyone ReticleStunGun allow

# Configurations (NEW!)
Change these in /NoReticle/fxmanifest.lua.

enable_give_all_weapons_command
hide_aircraft_reticle
