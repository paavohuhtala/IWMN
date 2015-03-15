# DIR_GAME and DIR_GAME_LOCAL must be set before running make
# DIR_GAME is the installation directory of the game (that contains Cities_Data)
# On Windows, DIR_GAME_LOCAL is equivalent to %localappdata%/Colossal Order/Cities_Skylines

PROJECT := IWMN

DIR_SOURCES := ./Source
SOURCES := $(shell find $(DIR_SOURCES) -name '*.cs')

DIR_BINARY := Bin
BINARY_NAME := $(PROJECT).dll
BINARY_PATH := $(DIR_BINARY)/$(BINARY_NAME)

DIR_GAME := $(DIR_GAME)/Cities_Data/Managed
DEPENDENCIES += Assembly-CSharp.dll
DEPENDENCIES += ICIties.dll
DEPENDENCIES += UnityEngine.dll
DEPENDENCIES += ColossalManaged.dll

DEPENDENCY_LIST := $(addprefix -r:, $(DEPENDENCIES))

$(BINARY_PATH): $(SOURCES)
	mkdir -p $(DIR_BINARY)
	mcs -debug /out:"$(BINARY_PATH)" /target:library /lib:"$(DIR_GAME)" $(DEPENDENCY_LIST) $(SOURCES)

# Warning: Depends on Windows and %localappdata%
DIR_ADDON := $(DIR_GAME_LOCAL)/Addons/Mods/$(PROJECT)
BINARY_INSTALL_PATH := $(DIR_ADDON)/$(BINARY_NAME)

install:
	cp "$(BINARY_PATH)" "$(BINARY_INSTALL_PATH)"
	cp "$(BINARY_PATH).mdb" "$(BINARY_INSTALL_PATH).mdb"

clean:
	rm -R $(DIR_BINARY)