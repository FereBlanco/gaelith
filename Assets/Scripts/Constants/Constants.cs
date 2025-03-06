using UnityEngine;

public static class Constants
{
    public static string TAG_PLAYER = "Player";
    public static string TAG_PORTAL_KEY = "PortalKey";
    public static string TAG_PORTAL_DOOR = "PortalDoor";

    public static float PLAYER_MOVE_TIME = 0.08f;
    public static float PLAYER_ROTATE_TIME = 0.15f;
    public static float PLAYER_PUSH_TIME = 0.1f;

    public static Vector3 SELF_RAYCAST_ORIGIN = new Vector3(0.0f, 0.5f, 0.0f);
    public static float STONE_MOVE_TIME = 0.1f;

    public static float SKY_LEVEL = 20.0f;
    public static float FLY_TIME = 3*1.0f;
    public static float FALL_TIME = 3*1.0f;
    public static float CELEBRATION_TIME = 1.5f;
    
    public static string TEXT_ROOM_NUMBER = "Room: ";
}