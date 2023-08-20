fx_version 'cerulean'
game 'gta5'

author 'ToastinYou'
description 'NoReticle removes the white reticle/dot when aiming a weapon.'
version '2.4.0'

server_script 'NoReticle.Server.net.dll'
client_script 'NoReticle.Client.net.dll'

-- Configurations -- Set to 'true'/'false' to enable/disable --

-- '/weapons' command which gives the player all weapons with max ammunition.
enable_give_all_weapons_command 'false'

hide_aircraft_reticle 'false'
