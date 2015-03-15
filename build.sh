#!/bin/sh
# Untested.

[[ $XDG_DATA_HOME ]] && HOME_PREFIX="$XDG_DATA_HOME" || HOME_PREFIX="~./local/share"

export DIR_GAME="${HOME_PREFIX}/Steam/SteamApps/common/Cities_Skylines"
export DIR_GAME_LOCAL="${HOME_PREFIX}/Colossal Order/Cities_Skylines"

make && make install