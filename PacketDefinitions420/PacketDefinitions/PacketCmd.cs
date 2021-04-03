﻿namespace PacketDefinitions420.PacketDefinitions
{
    /// <summary>
    /// Enum containing (not all, and often incorrectly) the byte definitions for possible packets to be sent to clients or sent to the server by clients.
    /// *NOTE* Will soon be depricated as the use of LeaguePackets will replace all functions in PacketNotifier that use this enum.
    /// </summary>
    public enum PacketCmd : short
    {
        PKT_KEY_CHECK = 0x00,
        PKT_S2C_RESTRICT_CAMERA_MOVEMENT = 0x06,
        PKT_C2S_HEART_BEAT = 0x08,
        PKT_C2S_SELL_ITEM = 0x09,
        PKT_UNPAUSE_GAME = 0x0A,
        PKT_S2C_REMOVE_ITEM = 0x0B,
        PKT_S2C_NEXT_AUTO_ATTACK = 0x0C,
        PKT_S2C_EDIT_MESSAGE_BOX_TOP = 0x0D,
        PKT_S2C_UNLOCK_CAMERA = 0x0E,
        PKT_S2C_ADD_XP = 0x10,
        PKT_S2C_END_SPAWN = 0x11,
        PKT_S2C_GAME_SPEED = 0x12,
        PKT_C2S_QUERY_STATUS_REQ = 0x14,
        PKT_S2C_SKILL_UP = 0x15,
        PKT_C2S_PING_LOAD_INFO = 0x16,
        PKT_S2C_CHANGE_SPELL = 0x17,
        PKT_S2C_FLOATING_TEXT = 0x18,
        PKT_S2C_FLOATING_TEXT_WITH_VALUE = 0x19,
        PKT_C2S_SWAP_ITEMS = 0x20,
        PKT_S2C_BEGIN_AUTO_ATTACK = 0x1A,
        PKT_S2C_CHAMPION_DIE2 = 0x1B,
        PKT_S2C_EDIT_BUFF = 0x1C,
        PKT_S2C_ADD_GOLD = 0x22,
        PKT_S2C_FOG_UPDATE2 = 0x23,
        PKT_S2C_MOVE_CAMERA = 0x25,
        PKT_S2C_SOUND_SETTINGS = 0x27,
        PKT_S2C_AVATAR_INFO = 0x2A,
        PKT_S2C_INHIBITOR_STATE = 0x2B,
        PKT_S2C_VIEW_ANS = 0x2C,
        PKT_C2S_VIEW_REQ = 0x2E,
        PKT_S2C_CHAMPION_RESPAWN = 0x2F,
        PKT_S2C_ADD_UNIT_FOW = 0x33,
        PKT_S2C_STOP_AUTO_ATTACK = 0x34,
        PKT_S2C_DELETE_OBJECT = 0x35, // not sure what this is, happens when turret leaves vision
        PKT_S2C_MESSAGE_BOX_TOP = 0x36,
        PKT_S2C_DESTROY_OBJECT = 0x38,
        PKT_C2S_SKILL_UP = 0x39,
        PKT_C2S_USE_OBJECT = 0x3A,
        PKT_S2C_SPAWN_PROJECTILE = 0x3B,
        PKT_S2C_SWAP_ITEMS = 0x3E,
        PKT_S2C_LEVEL_UP = 0x3F,
        PKT_S2C_ATTENTION_PING = 0x40,
        PKT_S2C_EMOTION = 0x42,
        PKT_S2C_PLAY_SOUND = 0x43,
        PKT_S2C_ANNOUNCE = 0x45,
        PKT_S2C_PLAYER_STATS = 0x46,
        PKT_C2S_AUTO_ATTACK_OPTION = 0x47,
        PKT_C2S_EMOTION = 0x48,
        PKT_S2C_HERO_SPAWN = 0x4C,
        PKT_S2C_FACE_DIRECTION = 0x50,
        PKT_S2C_LEAVE_VISION = 0x51,
        PKT_C2S_START_GAME = 0x52,
        PKT_S2C_SYNCH_VERSION = 0x54,
        PKT_S2C_BLUE_TIP = 0x55,
        PKT_C2S_SCOREBOARD = 0x56,
        PKT_C2S_ATTENTION_PING = 0x57,
        PKT_S2C_HIGHLIGHT_UNIT = 0x59,
        PKT_S2C_DESTROY_PROJECTILE = 0x5A,
        PKT_S2C_START_GAME = 0x5C,
        PKT_S2C_CHAMPION_DIE = 0x5E,
        PKT_S2C_MOVE_ANS = 0x61,
        PKT_S2C_START_SPAWN = 0x62,
        PKT_S2C_DASH = 0x64, PKT_C2S_CLIENT_READY = 0x64,
        PKT_S2C_DAMAGE_DONE = 0x65, PKT_S2C_LOAD_HERO = 0x65,
        PKT_S2C_LOAD_NAME = 0x66, PKT_S2C_MODIFY_SHIELD = 0x66,
        PKT_S2C_LOAD_SCREEN_INFO = 0x67,
        PKT_CHAT_BOX_MESSAGE = 0x68,
        PKT_S2C_SET_TARGET = 0x6A,
        PKT_S2C_SET_ANIMATION = 0x6B,
        PKT_C2S_BLUE_TIP_CLICKED = 0x6D,
        PKT_S2C_SHOW_PROJECTILE = 0x6E,
        PKT_S2C_BUY_ITEM_ANS = 0x6F,
        PKT_S2C_FREEZE_UNIT_ANIMATION = 0x71,
        PKT_C2S_MOVE_REQ = 0x72,
        PKT_S2C_SET_CAMERA_POSITION = 0x73,
        PKT_C2S_MOVE_CONFIRM = 0x77,
        PKT_S2C_REMOVE_BUFF = 0x7B,
        PKT_C2S_LOCK_CAMERA = 0x81,
        PKT_C2S_BUY_ITEM_REQ = 0x82,
        PKT_S2C_TOGGLE_INPUT_LOCKING_FLAG = 0x84,
        PKT_S2C_SET_COOLDOWN = 0x85,
        PKT_S2C_SPAWN_PARTICLE = 0x87,
        PKT_S2C_QUERY_STATUS_ANS = 0x88,
        PKT_S2C_EXPLODE_NEXUS = 0x89,            // <-- Building_Die?
        PKT_S2C_INHIBITOR_DEATH_ANIMATION = 0x89, // <--
        PKT_S2C_QUEST = 0x8C,
        PKT_C2S_EXIT = 0x8F,
        PKT_C2S_WORLD_SEND_GAME_NUMBER = 0x92, // <-- At least one of these is probably wrong
        PKT_S2C_WORLD_SEND_GAME_NUMBER = 0x92, // <--
        PKT_S2C_PING_LOAD_INFO = 0x95,
        PKT_S2C_CHANGE_CHARACTER_VOICE = 0x96,
        PKT_S2C_UPDATE_MODEL = 0x97,
        PKT_S2C_DISCONNECTED_ANNOUNCEMENT = 0x98,
        PKT_C2S_CAST_SPELL = 0x9A,
        PKT_S2C_TURRET_SPAWN = 0x9D,
        PKT_S2C_NPC_HIDE = 0x9E, // (4.18) not sure what this became
        PKT_S2C_SET_ITEM_STACKS = 0x9F,
        PKT_S2C_MESSAGE_BOX_RIGHT = 0xA0,
        PKT_PAUSE_GAME = 0xA1,
        PKT_S2C_REMOVE_MESSAGE_BOX_TOP = 0xA2,
        PKT_S2C_ANNOUNCE2 = 0xA3, // ? idk
        PKT_C2S_SURRENDER = 0xA4,
        PKT_S2C_SURRENDER_RESULT = 0xA5,
        PKT_S2C_REMOVE_MESSAGE_BOX_RIGHT = 0xA7,
        PKT_C2S_STATS_CONFIRM = 0xA8,
        PKT_S2C_ENABLE_FOW = 0xAB,
        PKT_S2C_SET_HEALTH = 0xAE,
        PKT_C2S_CLICK = 0xAF,
        PKT_S2C_SPELL_ANIMATION = 0xB0,
        PKT_S2C_EDIT_MESSAGE_BOX_RIGHT = 0xB1,
        PKT_S2C_SET_MODEL_TRANSPARENCY = 0xB2,
        PKT_S2C_BASIC_TUTORIAL_MESSAGE_WINDOW = 0xB3,
        PKT_S2C_REMOVE_HIGHLIGHT_UNIT = 0xB4,
        PKT_S2C_CAST_SPELL_ANS = 0xB5,
        PKT_S2C_ADD_BUFF = 0xB7,
        PKT_S2C_AFK_WARNING_WINDOW = 0xB8,
        PKT_S2C_OBJECT_SPAWN = 0xBA,
        PKT_S2C_HIDE_UI = 0xBC,
        PKT_C2S_SYNCH_VERSION = 0xBD,
        PKT_C2S_CHAR_LOADED = 0xBE,
        PKT_S2C_SET_TARGET2 = 0xC0,
        // Packet 0xC0 format is [Net ID 1] [Net ID 2], purpose still unknown
        PKT_S2C_GAME_TIMER = 0xC1,
        PKT_S2C_GAME_TIMER_UPDATE = 0xC2,
        PKT_S2C_CHAR_STATS = 0xC4,
        PKT_S2C_GAME_END = 0xC6,
        PKT_S2C_SURRENDER = 0xC9,
        PKT_C2S_QUEST_CLICKED = 0xCD,
        PKT_S2C_SHOW_HP_AND_NAME = 0xCE,
        PKT_S2C_LEVEL_PROP_SPAWN = 0xD0,
        PKT_S2C_LEVEL_PROP_ANIMATION = 0xD1,
        PKT_S2C_SET_CAPTURE_POINT = 0xD3,
        PKT_S2C_CHANGE_CRYSTAL_SCAR_NEXUS_HP = 0xD4,
        PKT_S2C_SET_TEAM = 0xD7,
        PKT_S2C_ATTACH_MINIMAP_ICON = 0xD8,
        PKT_S2C_DOMINION_POINTS = 0xD9,
        PKT_S2C_SET_SCREEN_TINT = 0xDB,
        PKT_S2C_CLOSE_GAME = 0xE5,
        PKT_C2S_SPELL_CHARGE_UPDATE = 0xE6,
        PKT_S2C_DEBUG_MESSAGE = 0xF7,
        PKT_S2C_MESSAGES_AVAILABLE = 0xF9,
        PKT_S2C_SET_ITEM_STACKS2 = 0xFD,
        PKT_S2C_EXTENDED = 0xFE,
        PKT_S2C_BATCH = 0xFF,

        PKT_S2C_SURRENDER_STATE = 0x10E,
        PKT_S2C_ON_ATTACK = 0x10F,
        PKT_S2C_CHAMPION_DEATH_TIMER = 0x117,
        PKT_S2C_SET_SPELL_ACTIVE_STATE = 0x118,
        PKT_S2C_RESOURCE_TYPE = 0x119,
        PKT_S2C_REPLACE_STORE_ITEM = 0x11C,
        PKT_S2C_CREATE_MONSTER_CAMP = 0x122,
        PKT_S2C_SPELL_EMPOWER = 0x125,
        PKT_S2C_NPC_DIE = 0x126,
        PKT_S2C_FLOATING_TEXT2 = 0x128,
        PKT_S2C_FORCE_TARGET_SPELL = 0x129,
        PKT_S2C_MOVE_CHAMPION_CAMERA_CENTER = 0x12B
    }
}
