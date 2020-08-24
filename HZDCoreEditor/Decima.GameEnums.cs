using System.Runtime.InteropServices;

namespace Decima
{
    using int8 = System.SByte;
    using uint8 = System.Byte;
    using int16 = System.Int16;
    using uint16 = System.UInt16;
    using int32 = System.Int32;
    using uint32 = System.UInt32;
    using int64 = System.Int64;
    using uint64 = System.UInt64;

    using wchar = System.Int16;
    using HalfFloat = System.UInt16;

    using MaterialType = System.UInt16;
    using AnimationTagID = System.UInt32;
    using AnimationStateID = System.UInt32;
    using AnimationEventID = System.UInt32;
    using PhysicsCollisionFilterInfo = System.UInt32;

    static partial class GameData
    {
        [RTTI.Serializable(0x860E4B096602077A, 0x1)]
        public enum AnimationMountStateLogic : int8
        {
            Tag = 0,
            Event = 1,
        }

        [RTTI.Serializable(0x4048C886DA6FDBFA, 0x4)]
        public enum ClanRole : int32
        {
            BASIC = 0,
            OFFICER = 1,
        }

        [RTTI.Serializable(0x789431BD7D118A93, 0x4)]
        public enum ClanStatus : int32
        {
            OK = 0,
            ERROR = 1,
        }

        [RTTI.Serializable(0x5F0FED2FAA8AB276, 0x4)]
        public enum CrowdEventType : int32
        {
            Danger = 0,
            Interest = 1,
            Direct_Crowd = 2,
        }

        [RTTI.Serializable(0xCCB10ECFF2AFECD5, 0x4)]
        public enum EAAMode : int32
        {
            None = 0,
            FXAA = 1,
            SMAA_1X = 2,
            TAA = 3,
            Default = -1,
        }

        [RTTI.Serializable(0x68055FAFA962FEC4, 0x4)]
        public enum EAIAttackType : int32
        {
            Area = 3,
            Ballistic = 2,
            Contact = 0,
            Line = 1,
            Line_DLC_0 = 4,
            Ballistic_DLC_0 = 5,
        }

        [RTTI.Serializable(0x59B105389B2BDB6, 0x1)]
        public enum EAIBehaviorGroupMemberNavmeshPlacmentType : int8
        {
            FindRandomPointInRangeOnNavmesh = 0,
            FindRandomPointInRangeInAirNav = 1,
        }

        [RTTI.Serializable(0x97E543FE36E90C98, 0x1)]
        public enum EAIBodyAlignmentMode : int8
        {
            TurnUsingAnimation = 0,
            TurnWithoutAnimation = 1,
            NoTurnWhileOperating = 2,
        }

        [RTTI.Serializable(0x491B23684391A9E6, 0x4)]
        public enum EAICover : int32
        {
            LOS_FULLY_BLOCKED = 3,
            LOS_IF_NOT_CROUCHED = 2,
            LOS_IF_NOT_PRONE = 1,
            LOS_ALWAYS = 0,
        }

        [RTTI.Serializable(0x8C8262D1BF2F10D9, 0x4)]
        public enum EAIDangerAreaType : int32
        {
            Unspecified = 0,
            Electricity = 1,
            Fire = 2,
            Explosion = 3,
            Vehicle = 4,
            Cryo = 5,
            Sequence = 6,
        }

        [RTTI.Serializable(0x71A853F25377910E, 0x4)]
        public enum EAIPatrolPathType : int32
        {
            Loop = 0,
            Once = 1,
            BackForth = 2,
            BackForthOnce = 3,
        }

        [RTTI.Serializable(0x463EA09673894D77, 0x1)]
        public enum EAIRoadUsableBy : int8
        {
            None = 0,
            Humans = 1,
            Robots = 2,
            Player = 4,
            All = 7,
        }

        [RTTI.Serializable(0x962FD3E9FAC0FD00, 0x1)]
        public enum EActivateConditionRelation : int8
        {
            And = 0,
            Or = 1,
            Override = 2,
        }

        [RTTI.Serializable(0x13E84224B65188AC, 0x4)]
        public enum EActiveView : int32
        {
            None = -1,
            Default = 0,
            ThirdPerson = 1,
            FirstPerson = 2,
        }

        [RTTI.Serializable(0x5034A55A46569ACC, 0x4)]
        public enum EActivityFeedTriggerAction : int32
        {
            None = 0,
            JoinPlaylist = 1,
            JoinFriend = 2,
            OpenWebsite = 3,
            OpenStore = 4,
            OpenSP = 5,
            OpenMP = 6,
        }

        [RTTI.Serializable(0x85E4B88E07781A6C, 0x4)]
        public enum EActivityMedalType : int32
        {
            Golden = 3,
            Silver = 2,
            Bronze = 1,
        }

        [RTTI.Serializable(0x3FE9099AA4CBF5C7, 0x4)]
        public enum EAlertLevel : int32
        {
            invalid = 0,
            no_threats = 1,
            presence_suspected = 2,
            presence_confirmed = 3,
            threats_identified = 4,
            combat = 5,
            under_attack = 6,
            getting_hit = 7,
        }

        [RTTI.Serializable(0x5616D30C32DB4214, 0x4)]
        public enum EAlertPartType : int32
        {
            Alert = 0,
            Array = 1,
            Text = 2,
            FieldList = 4,
            Field = 5,
            RTTIObject = 6,
        }

        [RTTI.Serializable(0x9A5BF5EE6FA9DCBB, 0x4)]
        public enum EAlertType : int32
        {
            Normal = 0,
            TerminateProcess = 1,
            LogOnly = 2,
        }

        [RTTI.Serializable(0x36F48A22F8EBDD81, 0x4)]
        public enum EAlphaDepth : int32
        {
            Disable = 7,
            Never = 0,
            Less = 1,
            Equal = 2,
            LessOrEqual = 3,
            Greater = 4,
            NotEqual = 5,
            GreaterOrEqual = 6,
        }

        [RTTI.Serializable(0x427FAED60FCC9B20, 0x4)]
        public enum EAmmoCostType : int32
        {
            Ammo_Per_Shot = 0,
            Ammo_Per_Burst = 1,
        }

        [RTTI.Serializable(0x440BE23BC83853D1, 0x4)]
        public enum EAmmoSettings : int32
        {
            AmmoLow = 0,
            AmmoNormal = 1,
            AmmoHigh = 2,
        }

        [RTTI.Serializable(0x2E89FD2E3826E7C1, 0x1)]
        public enum EAmmoTetherState : int8
        {
            Invalid_Tether_State = -1,
            Untethered__Idle = 0,
            Untethered__Searching = 1,
            Tethered__Loading = 2,
            Tethered__Idle = 3,
        }

        [RTTI.Serializable(0xBFAA0791E2E333E2, 0x4)]
        public enum EAnimationActionAction : int32
        {
            Start = 0,
            Stop = 1,
            Trigger = 2,
        }

        [RTTI.Serializable(0x347F8FCE8794808D, 0x4)]
        public enum EAnimationDamageType : int32
        {
            none = -1,
            projectile = 0,
            explosion = 2,
            fire = 1,
            electricity = 3,
        }

        [RTTI.Serializable(0x532D9C9E326793FF, 0x4)]
        public enum EAnimationDirection : int32
        {
            any = -1,
            front = 0,
            back = 1,
        }

        [RTTI.Serializable(0x93905C311BFB983B, 0x1)]
        public enum EAnimationTransitionCollisionPath : int8
        {
            None = 0,
            FromAnimationEvents = 1,
            Automatic = 2,
        }

        [RTTI.Serializable(0x47367503C856B39E, 0x4)]
        public enum EAnnotationPrimitiveTag : int32
        {
            Climbable = 0,
            VerticalHandsOutside = 1,
            VerticalHandsInside = 2,
            Balanceable = 3,
            Ziplineable = 4,
            Disallow_Aircontrol = 5,
            AllowMounting = 6,
            Unstable = 7,
            CannotRelease = 8,
            CannotClimbOver = 9,
        }

        [RTTI.Serializable(0x102D6E8F62A8B693, 0x4)]
        public enum EApertureShape : int32
        {
            Polygon = 0,
            Circle = 1,
            Texture = 2,
        }

        [RTTI.Serializable(0x6F43EC706669C28B, 0x4)]
        public enum EArcTargetType : int32
        {
            None = 0,
            Entity = 1,
            World = 2,
            Air = 3,
        }

        [RTTI.Serializable(0x5214C0FE20D34437, 0x1)]
        public enum EAreaOfInvestigation : int8
        {
            no_area = 0,
            around_throwable = 1,
            around_instigator = 2,
        }

        [RTTI.Serializable(0xF4E47A9A29E7FF37, 0x4)]
        public enum EAttachmentType : int32
        {
            Scope = 0,
            SecFunction = 1,
        }

        [RTTI.Serializable(0xF54F8ACE848BCBDD, 0x1)]
        public enum EAttackEventLinkType : int8
        {
            Automatic = 0,
            FirstMajorLink = 1,
            MinorLink = 2,
            MajorLink = 3,
            PassThrough = 4,
        }

        [RTTI.Serializable(0x7F43C3702461B9FC, 0x1)]
        public enum EAttackEventType : int8
        {
            Unspecified = 0,
            DeliberatelyEmpty = 1,
            SelfInflicted = 2,
            Environmental = 3,
            Physics = 4,
            SequenceEvent = 5,
            AttackEvent = 6,
            WeaponBurst = 7,
            MeleeAttack = 8,
            Explosion = 9,
            DamageArea = 10,
            ImpactDamage = 11,
            GraphNode = 12,
            ToBeReplaced = 13,
        }

        [RTTI.Serializable(0x54CC71356FD97E32, 0x1)]
        public enum EAttackNodePolicy : int8
        {
            AttackRoot = 0,
            AttackHighest = 1,
        }

        [RTTI.Serializable(0x1098E9129E984E05, 0x4)]
        public enum EAwarenessType : int32
        {
            Unaware = 0,
            Identified = 1,
            Suspected = 2,
        }

        [RTTI.Serializable(0x6E49E82C3DCB73A8, 0x4)]
        public enum EAxisType : int32
        {
            None = 0,
            CameraYawOnly = 1,
            x = 2,
            y = 3,
            z = 4,
            emitter = 5,
            velocity = 6,
            VelocityYawOnly = 7,
        }

        [RTTI.Serializable(0x482AAA3B75AA1B48, 0x1)]
        public enum EBehaviorEscalation : int8
        {
            no_escalation = 0,
            escalate_to_suspicious = 1,
            escalate_to_combat = 2,
        }

        [RTTI.Serializable(0xEC4A2415DFFFF2D4, 0x4)]
        public enum EBehaviourOnHide : int32
        {
            Success = 0,
            Fail = 1,
            Hide = 2,
        }

        [RTTI.Serializable(0x70A106565C79D279, 0x1)]
        public enum EBooleanFactConditionMode : int8
        {
            Global = 0,
            Player = 1,
            Context = 2,
        }

        [RTTI.Serializable(0x3F2E60ED3AA8DE21, 0x1)]
        public enum EBooleanFactOperator : int8
        {
            And = 0,
            Or = 1,
        }

        [RTTI.Serializable(0xCB486D8385A352E4, 0x1)]
        public enum EBuddySpawnRequestMode : int8
        {
            None = 0,
            SpawnMarker = 1,
            Spawnpoint = 2,
            LastKnownPosition = 3,
            NearPlayer = 4,
        }

        [RTTI.Serializable(0x1A83801E42D51EA4, 0x1)]
        public enum EBuddyState : int8
        {
            Unregistered = 0,
            Inactive = 1,
            Spawning = 2,
            Active = 3,
            Dead = 4,
            Despawned = 5,
        }

        [RTTI.Serializable(0x249409690625A5F2, 0x4)]
        public enum EButton : int32
        {
            Invalid = -1,
            Right = 13,
            Left = 15,
            Up = 12,
            Down = 14,
            Triangle = 0,
            Circle = 1,
            Cross = 2,
            Square = 3,
            Shoulder_Left_1 = 6,
            Shoulder_Left_2 = 4,
            Shoulder_Right_1 = 7,
            Shoulder_Right_2 = 5,
            Left_Analog = 10,
            Right_Analog = 11,
            Select = 9,
            Start = 8,
            Touch_pad = 21,
            Touch_pad_left = 22,
            Touch_pad_right = 23,
            Xbox_None = 30,
            Xbox_Menu = 31,
            Xbox_View = 32,
            Xbox_A = 33,
            Xbox_B = 34,
            Xbox_X = 35,
            Xbox_Y = 36,
            Xbox_Dpad_Up = 37,
            Xbox_Dpad_Down = 38,
            Xbox_Dpad_Left = 39,
            Xbox_Dpad_Right = 40,
            Xbox_Left_Shoulder = 41,
            Xbox_Right_Shoulder = 42,
            Xbox_Left_Trigger = 43,
            Xbox_Right_Trigger = 44,
            Xbox_Left_Thumbstick = 45,
            Xbox_Right_Thumbstick = 46,
            Steam_None = 47,
            Steam_Start = 48,
            Steam_Select = 49,
            Steam_A = 50,
            Steam_B = 51,
            Steam_X = 52,
            Steam_Y = 53,
            Steam_Lpad_Up = 54,
            Steam_Lpad_Down = 55,
            Steam_Lpad_Left = 56,
            Steam_Lpad_Right = 57,
            Steam_Left_Bumper = 58,
            Steam_Right_Bumper = 59,
            Steam_Left_Trigger = 60,
            Steam_Right_Trigger = 61,
            Steam_Left_Thumbstick = 62,
            Steam_Right_PAD = 63,
            Steam_Right_BackPanel = 64,
            Steam_Left_BackPanel = 65,
            Key_None = 66,
            Key_Esc = 67,
            Key_Plus = 68,
            Key_Minus = 69,
            Key_Space = 70,
            Key_Oquote = 71,
            Key_Cquote = 72,
            Key_Lhook = 73,
            Key_Rhook = 74,
            Key_Bslash = 75,
            Key_Fslash = 76,
            Key_Semicolon = 77,
            Key_Comma = 78,
            Key_Dot = 79,
            Key_Enter = 80,
            Key_Backspace = 81,
            Key_Tab = 82,
            Key_Left = 83,
            Key_Right = 84,
            Key_Up = 85,
            Key_Down = 86,
            Key_Home = 87,
            Key_End = 88,
            Key_Pgup = 89,
            Key_Pgdn = 90,
            Key_Ins = 91,
            Key_Del = 92,
            Key_Pad_Mul = 93,
            Key_Pad_Div = 94,
            Key_Pad_Plus = 95,
            Key_Pad_Minus = 96,
            Key_Pad_Enter = 97,
            Key_Pad_0 = 98,
            Key_Pad_1 = 99,
            Key_Pad_2 = 100,
            Key_Pad_3 = 101,
            Key_Pad_4 = 102,
            Key_Pad_5 = 103,
            Key_Pad_6 = 104,
            Key_Pad_7 = 105,
            Key_Pad_8 = 106,
            Key_Pad_9 = 107,
            Key_Pad_Del = 108,
            Key_Capslock = 109,
            Key_Printscreen = 110,
            Key_Scrolllock = 111,
            Key_Numlock = 112,
            Key_Pause = 113,
            Key_Lalt = 114,
            Key_Ralt = 115,
            Key_Lctrl = 116,
            Key_Rctrl = 117,
            Key_Lshift = 118,
            Key_Rshift = 119,
            Key_Win_Lwinkey = 120,
            Key_Win_Rwinkey = 121,
            Key_Win_Context = 122,
            Key_F1 = 123,
            Key_F2 = 124,
            Key_F3 = 125,
            Key_F4 = 126,
            Key_F5 = 127,
            Key_F6 = 128,
            Key_F7 = 129,
            Key_F8 = 130,
            Key_F9 = 131,
            Key_F10 = 132,
            Key_F11 = 133,
            Key_F12 = 134,
            Key_0 = 135,
            Key_1 = 136,
            Key_2 = 137,
            Key_3 = 138,
            Key_4 = 139,
            Key_5 = 140,
            Key_6 = 141,
            Key_7 = 142,
            Key_8 = 143,
            Key_9 = 144,
            Key_A = 145,
            Key_B = 146,
            Key_C = 147,
            Key_D = 148,
            Key_E = 149,
            Key_F = 150,
            Key_G = 151,
            Key_H = 152,
            Key_I = 153,
            Key_J = 154,
            Key_K = 155,
            Key_L = 156,
            Key_M = 157,
            Key_N = 158,
            Key_O = 159,
            Key_P = 160,
            Key_Q = 161,
            Key_R = 162,
            Key_S = 163,
            Key_T = 164,
            Key_U = 165,
            Key_V = 166,
            Key_W = 167,
            Key_X = 168,
            Key_Y = 169,
            Key_Z = 170,
            Key_Caps_Toggle = 181,
            Key_Num_Toggle = 182,
            Key_Scroll_Toggle = 183,
            Mouse_Left = 187,
            Mouse_Right = 188,
            Mouse_Middle = 189,
            Mouse_XButton1 = 190,
            Mouse_XButton2 = 191,
            Mouse_WheelUp = 192,
            Mouse_WheelDown = 193,
            Virtual_Mouse_Left_Button = 186,
        }

        [RTTI.Serializable(0xF371DA5B2488A6A0, 0x4)]
        public enum ECameraShotType : int32
        {
            Close_Up = 0,
            Close_Up_Variant_1 = 1,
            Close_Up_Variant_2 = 2,
            Close_Up_Variant_3 = 3,
            Medium_Shot = 4,
            Medium_Shot_Variant_1 = 5,
            Medium_Shot_Variant_2 = 6,
            Medium_Shot_Variant_3 = 7,
            Over_the_Shoulder = 8,
            Over_the_Shoulder_Variant_1 = 9,
            Over_the_Shoulder_Variant_2 = 10,
            Over_the_Shoulder_Variant_3 = 11,
            Wide_Variant_1 = 12,
            Wide_Variant_2 = 13,
            Wide_Variant_3 = 14,
        }

        [RTTI.Serializable(0x5F21A55D24C37829, 0x4)]
        public enum ECameraTransitionFunction : int32
        {
            TransitionLinear = 0,
            TransitionSmoothStep = 1,
        }

        [RTTI.Serializable(0xBB6064EB918DCF9F, 0x4)]
        public enum ECaptureAndHoldAreaState : int32
        {
            Invalid = -1,
            Neutralized = 0,
            Neutralizing = 1,
            Captured = 2,
            Capturing = 3,
        }

        [RTTI.Serializable(0x2CFDE99B47C1C0BF, 0x4)]
        public enum ECareer : int32
        {
            None = -1,
            Scout = 0,
            Soldier = 1,
            Support = 2,
        }

        [RTTI.Serializable(0x536DAE6AACF5EF1D, 0x4)]
        public enum ECareerSettings : int32
        {
            Disabled = 0,
            HGH_Only = 1,
            VSA_Only = 2,
            Enabled = 3,
        }

        [RTTI.Serializable(0x8F464FA4505EF3A0, 0x4)]
        public enum ECarryModes : int32
        {
            INVALID = -1,
            IDLE = 0,
            TACTICAL = 1,
            COMBAT = 2,
        }

        [RTTI.Serializable(0x9007B69E8F69A1C2, 0x4)]
        public enum ECastingShadowQuality : int32
        {
            Off = 0,
            Low = 1,
            Medium = 2,
            High = 3,
            Ultra = 4,
        }

        [RTTI.Serializable(0x40C30D70EEF1A1C9, 0x1)]
        public enum ECheckQuestItems : int8
        {
            ResourceDefault = 0,
            NoQuestItems = 1,
            OnlyQuestItems = 2,
            AllItems = 3,
        }

        [RTTI.Serializable(0xCE25EA36C86B63C3, 0x4)]
        public enum EChildrenClipMode : int32
        {
            _0 = 0,
            clip = 1,
            noclip = 2,
        }

        [RTTI.Serializable(0x9D1424E800EF5E54, 0x4)]
        public enum EClanMatchOutcome : int32
        {
            ISA_WON = 0,
            HGH_WON = 1,
            DRAW = 2,
            NO_GAME = 3,
        }

        [RTTI.Serializable(0x249D27C69B4F9330, 0x4)]
        public enum ECloseCombatSettings : int32
        {
            CloseCombatOn = 0,
            CloseCombatOff = 1,
        }

        [RTTI.Serializable(0x403DF9094A09EB33, 0x4)]
        public enum ECollectableRobotEntryType : int32
        {
            robot = 0,
            corrupted = 1,
            cauldron_corrupted = 2,
        }

        [RTTI.Serializable(0x7B76E446EE953A2B, 0x4)]
        public enum ECollisionType : int32
        {
            ReadOnly = 0,
            Full = 1,
            WriteAfter = 2,
            None = 3,
        }

        [RTTI.Serializable(0xE1D2D4678AE10A66, 0x4)]
        public enum EColorizeBlendMode : int32
        {
            Lerp = 0,
            ColorCorrect = 1,
        }

        [RTTI.Serializable(0x319D1E5C1BDD4364, 0x4)]
        public enum ECommandPriority : int32
        {
            unspecified = 0,
            blind_following = 1,
            follow_orders = 2,
            non_battle_initiative = 3,
            idle = 4,
        }

        [RTTI.Serializable(0x54ADABAA2A86058E, 0x4)]
        public enum EComparator : int32
        {
            Equals = 0,
            NotEquals = 1,
            GreaterThan = 2,
            GreaterThanEquals = 3,
            LessThan = 4,
            LessThanEquals = 5,
        }

        [RTTI.Serializable(0x5B36ABC4939560FE, 0x4)]
        public enum ECompletionAutoRotate : int32
        {
            None = 0,
            UseObjectRotation = 1,
            RotateToLight = 2,
            RotateToCentre = 3,
        }

        [RTTI.Serializable(0xEFF1F84E194DBE8B, 0x4)]
        public enum EComputeThreadDistribution : int32
        {
            MaxThreads_1D = 0,
            MaxThreads_2D = 1,
            MaxThreads_3D = 2,
        }

        [RTTI.Serializable(0x618022182A056D6A, 0x4)]
        public enum EContactType : int32
        {
            Colliding_and_resting = 0,
            Colliding_and_bouncing = 1,
            Sliding = 2,
            Rolling = 3,
        }

        [RTTI.Serializable(0xEB3BBFB950180268, 0x1)]
        public enum EContextualActionAnimationActions : int8
        {
            Trigger_at_start = 0,
            Keep_active = 1,
        }

        [RTTI.Serializable(0xB70BBDE506C8C3D4, 0x1)]
        public enum EContextualActionButtonType : int8
        {
            Single_button_press = 0,
            Continuous_button_press = 1,
        }

        [RTTI.Serializable(0xDF83412C91BAAE4C, 0x1)]
        public enum EContextualActionDeviceFunctionType : int8
        {
            PrimaryContextualAction = 0,
            SecondaryContextualAction = 1,
            TertiaryContextualAction = 2,
        }

        [RTTI.Serializable(0x1E9461F7EFCB00B4, 0x1)]
        public enum EContextualActionSwitchToWeapon : int8
        {
            Switch_to_MeleeWeapon = 0,
            Switch_to_Nothing = 1,
        }

        [RTTI.Serializable(0xD61AF03774CC5572, 0x1)]
        public enum EContextualActionTriggerAction : int8
        {
            Trigger_at_start = 0,
            Trigger_at_event = 1,
            Trigger_on_mount = 2,
        }

        [RTTI.Serializable(0xD63C59AFC3859631, 0x4)]
        public enum EContextualOrderError : int32
        {
            ContextualErrorNone = 0,
            ContextualErrorInvalidTarget = 1,
            ContextualErrorInvalidTargetPosition = 2,
            ContextualErrorOutOfRange = 3,
            ContextualErrorNoTarget = 4,
            ContextualErrorOrderUnavailable = 5,
            ContextualErrorOWLDeployBlocked = 6,
            ContextualErrorOWLRecharging = 7,
            ContextualErrorOWLAwaitingReturn = 8,
            ContextualErrorZiplineInvalidAngle = 9,
            ContextualErrorZiplineTooClose = 10,
            ContextualErrorZiplinePathBlocked = 11,
            ContextualErrorZiplineInvalidStance = 12,
            ContextualErrorOtherActionsInProgress = 13,
            ContextualErrorZiplineInProgress = 14,
        }

        [RTTI.Serializable(0x7157E789D46864B, 0x4)]
        public enum EControlType : int32
        {
            None = 0,
            Movement = 1,
            Rotation = 2,
            Buttons = 4,
            Motion = 8,
            Aim = 16,
            InventorySelection = 32,
            All = 63,
        }

        [RTTI.Serializable(0xCC7A9EEA66BA9228, 0x4)]
        public enum EControllerButtonType : int32
        {
            buttonTypeNormal = 0,
            buttonTypeHold = 1,
            buttonTypeAttack = 2,
            buttonTypeAiming = 3,
            buttonTypeScan = 4,
            buttonTypeMenu = 5,
            buttonTypeAimSwitch = 6,
            buttonTypeUseLocation = 7,
            buttonTypeAll = 8,
        }

        [RTTI.Serializable(0xA9AD31C8735D73F6, 0x4)]
        public enum EControllerScheme : int32
        {
            Dual_Shock = 0,
            Remote_Play = 1,
        }

        [RTTI.Serializable(0x63C6860EB2CEA63D, 0x4)]
        public enum ECrashMoverTargeting : int32
        {
            Unknown = 0,
            Manual = 1,
            Damager = 2,
            AutomaticMarker = 3,
            Player = 4,
        }

        [RTTI.Serializable(0xA3DC8D326D0794A4, 0x4)]
        public enum ECreateAsChild : int32
        {
            If_mover_requires_parent = 0,
            Autonomous_child = 1,
            Child_owned_by_parent = 2,
        }

        [RTTI.Serializable(0x38E4279787DF9832, 0x4)]
        public enum ECreateEntityFactionOverride : int32
        {
            None = 0,
            Entity = 1,
            Activator = 2,
            Instigator = 3,
        }

        [RTTI.Serializable(0xFF5F1533428EA2CC, 0x4)]
        public enum ECreateEntityLifetime : int32
        {
            Automatic = 0,
            Scene = 1,
            OtherEntity = 2,
            Dispensable = 3,
            Manual = 4,
        }

        [RTTI.Serializable(0xC0D76DF4E8676FD7, 0x1)]
        public enum ECrosshairBulletIndicatorType : int8
        {
            Invalid = 0,
            MagazineSize = 1,
            BurstSize = 2,
        }

        [RTTI.Serializable(0x7D03B9DE60140197, 0x1)]
        public enum ECrosshairPartAnimationTrigger : int8
        {
            Invalid = 0,
            ShotOnTarget = 1,
            PerfectAccuracy = 2,
            WorstAccuracy = 3,
            ZeroCharge = 4,
            FiringCharge = 5,
            FullCharge = 6,
            Overcharge = 7,
            AmmoCharged = 8,
            AmmoChargedFire = 9,
            PreFire = 10,
            Fire = 11,
            PerfectFire = 12,
            TargetAquired = 13,
            TargetLost = 14,
            OutOfRange = 15,
            InRange = 16,
        }

        [RTTI.Serializable(0x4C91C3DAFB67B482, 0x1)]
        public enum ECrowdImpostorAnimationState : int8
        {
            Walk = 0,
            Stand = 1,
            Sit = 2,
            Crouch = 3,
        }

        [RTTI.Serializable(0x1088866198E83A92, 0x1)]
        public enum ECubemapZoneDataStorageMode : int8
        {
            NonStreamingData = 0,
            StreamingData = 1,
        }

        [RTTI.Serializable(0xE440A7BF5092B8C3, 0x4)]
        public enum ECull : int32
        {
            CW = 1,
            CCW = 2,
            Off = 0,
        }

        [RTTI.Serializable(0x451A5B8C8AAFF841, 0x4)]
        public enum EDamageFlags : int32
        {
            Empty = 0,
            NoEffects = 1,
            Kill = 2,
            KillPart = 4,
            KeepAlive = 8,
            OneShot = 16,
            User1 = 32,
            User2 = 64,
            User3 = 128,
            User4 = 256,
            User5 = 512,
            User6 = 1024,
            User7 = 2048,
            User8 = 4096,
            User9 = 8192,
            User10 = 16384,
            User11 = 32768,
            User12 = 65536,
            User13 = 131072,
            User14 = 262144,
            User15 = 524288,
            User16 = 1048576,
            User17 = 2097152,
            User18 = 4194304,
            User19 = 8388608,
            User20 = 16777216,
            User21 = 33554432,
            User22 = 67108864,
            User23 = 134217728,
            User24 = 268435456,
            User25 = 536870912,
            User26 = 1073741824,
            User27 = -2147483648,
        }

        [RTTI.Serializable(0x1635C5268F821B8, 0x4)]
        public enum EDamageFlagsGameExported : int32
        {
            Empty = 0,
            HeadShot = 2048,
            PassedThroughOwnPlacedObject = 4096,
            WeaponWasZoomed = 65536,
            DamagerWasInCover = 131072,
            DamagerWasCrouched = 262144,
            DamagerWasJumping = 524288,
            DamagerWasSliding = 1048576,
            DamagerWasParkouring = 2097152,
            DamagerWasStealthed = 4194304,
            ContentDefined1 = 8388608,
            ContentDefined2 = 16777216,
            ContentDefined3 = 33554432,
            ContentDefined4 = 67108864,
            ContentDefined5 = 134217728,
        }

        [RTTI.Serializable(0x6261180EFFAE400F, 0x4)]
        public enum EDamageModifierTypeFilter : int32
        {
            Any = 0,
            Equals = 1,
            Not_Equals = 2,
        }

        [RTTI.Serializable(0x55BE62549B690A95, 0x4)]
        public enum EDebugFlagDefault : int32
        {
            Action = 0,
            ElseAction = 1,
        }

        [RTTI.Serializable(0xA13DC9447CD9C177, 0x4)]
        public enum EDecalAlignment : int32
        {
            AlignToImpactDirection = 0,
            AlignToWorldUpAxis = 1,
        }

        [RTTI.Serializable(0x6C0354EF63B06714, 0x4)]
        public enum EDecalProjectionMode : int32
        {
            ProjectImpactNormal = 0,
            ProjectImpactDir = 1,
            ProjectSurfaceHeuristic = 2,
        }

        [RTTI.Serializable(0x9DF7BD5502BF3DD2, 0x4)]
        public enum EDecalVariableSource : int32
        {
            None = 0,
            Fade = 1,
        }

        [RTTI.Serializable(0x4836DAA96F71C95D, 0x1)]
        public enum EDefaultDataStorageType : int8
        {
            None = 0,
            Binary = 1,
            ObjectRef = 2,
            UUIDRef = 3,
        }

        [RTTI.Serializable(0x6FD217A2B7F522E9, 0x4)]
        public enum EDelayLineTapIndex : int32
        {
            Tap_0 = 0,
            Tap_1 = 1,
            Tap_2 = 2,
            Tap_3 = 3,
        }

        [RTTI.Serializable(0xF05C6CF2742249E, 0x4)]
        public enum EDensityJobBakeType : int32
        {
            NoShaderCompilation = 0,
            FullConversion = 1,
        }

        [RTTI.Serializable(0x7718FC8906307384, 0x4)]
        public enum EDensityJobType : int32
        {
            SingleMap = 0,
            Full = 1,
        }

        [RTTI.Serializable(0xD06EE641B09C84DC, 0x4)]
        public enum EDeviceFunction : int32
        {
            Invalid = -1,
            PrimaryFire = 4,
            SwitchFire = 5,
            NextAmmo = 6,
            PreviousAmmo = 7,
            MeleeWeaponPrimaryAttack = 8,
            MeleeWeaponSecondaryAttack = 9,
            ZoomSwitch = 10,
            ZoomModeSwitch = 11,
            Aim = 12,
            SprintToggle = 13,
            Jump = 14,
            Use = 15,
            Relocate = 16,
            Loot = 17,
            PickupWeapon = 18,
            QuickThrowGesture = 19,
            Reload = 20,
            Crouch = 21,
            Slide = 22,
            Cover = 23,
            Vault = 24,
            DiveRoll = 25,
            DropWeapon = 26,
            NextWeapon = 27,
            SwimUnderWater = 28,
            ToggleFlashlight = 29,
            ToggleSlowMo = 30,
            CloseCombat = 31,
            Block = 32,
            Suicide = 33,
            LockAim = 34,
            SkipSequence = 35,
            SpectatorToggleSpawnMenu = 36,
            DropFromLedge = 37,
            DropToLedge = 38,
            Heal = 39,
            GrabLeftHand = 40,
            GrabRightHand = 41,
            CounterBucking1 = 42,
            CounterBucking2 = 43,
            PrimaryContextualAction = 44,
            SecondaryContextualAction = 45,
            TertiaryContextualAction = 46,
            RequestVoiceComm = 47,
            AAGunFirePrimary = 48,
            AAGunFireSecondary = 49,
            AAGunZoomSwitch = 50,
            SelectUp = 51,
            SelectDown = 52,
            SelectLeft = 53,
            SelectRight = 54,
            CharacterScreenCampaign = 55,
            CharacaterScreenOnline = 56,
            IngameMainMenu = 59,
            ShowHud = 60,
            OptionScreenCampaign = 57,
            OptionScreenOnline = 58,
            MountHorse = 61,
            DismountHorse = 62,
            DismountHorseSpecial = 63,
            MountSpeedUp = 64,
            MountSpeedDown = 65,
            MountRangedAttack = 66,
            MountMeleeAttack = 67,
            MountSecondaryMeleeAttack = 68,
            CallHorse = 69,
            InventoryNextWeapon = 70,
            InventoryStowWeapon = 71,
            InventorySelection = 72,
            InventoryQuickSelection_1 = 73,
            InventoryQuickSelection_2 = 74,
            InventoryQuickSelection_3 = 75,
            InventoryQuickSelection_4 = 76,
            InventorySelectToolNext = 77,
            InventorySelectToolPrev = 78,
            InventoryUseTool = 79,
            InventoryAmmoCraft = 80,
            ProtoRight = 81,
            ProtoLeft = 82,
            ProtoUp = 83,
            ProtoDown = 84,
            ProtoCross = 85,
            ProtoSquare = 86,
            ProtoTriangle = 87,
            ProtoCircle = 88,
            ProtoShoulderLeft1 = 89,
            ProtoShoulderLeft2 = 90,
            ProtoShoulderRight1 = 91,
            ProtoShoulderRight2 = 92,
            ProtoLeftAnalog = 93,
            ProtoRightAnalog = 94,
            ProtoSelect = 95,
            ProtoStart = 96,
            Tag = 97,
            Untag = 98,
            UntagAll = 99,
            Focus = 100,
            FocusTagStatusInfo = 101,
            FocusWeaponSelect = 102,
            FocusUp = 103,
            FocusDown = 104,
            FocusLeft = 105,
            FocusRight = 106,
            BulletTime = 107,
            LureEnemy = 108,
            AudiologToggle = 109,
            Confirm = 110,
            Cancel = 111,
            CampfireUserSave = 112,
            Keyboard_MovementForward = 113,
            Keyboard_MovementBackward = 114,
            Keyboard_MovementLeft = 115,
            Keybaord_MovementRight = 116,
            Keyboard_WalkRunSwitch = 117,
            Keyboard_HeavyAttackToggle = 118,
            MountSpeedUpPC = 125,
            JumpPC = 126,
            RelocatePC = 127,
            VaultPC = 128,
            SkipSequencePC = 129,
            SuicidePC = 130,
            ProtoCrossPC = 131,
        }

        [RTTI.Serializable(0x9ABB4365281257C4, 0x4)]
        public enum EDifficulty : int32
        {
            None = -1,
            VeryEasy = 0,
            Easy = 1,
            Medium = 2,
            Hard = 3,
            UltraHard = 4,
            Impossible = 5,
        }

        [RTTI.Serializable(0x105E1A1438E0B3A4, 0x4)]
        public enum EDisableItem : int32
        {
            Ignore = 0,
            Disabled = 1,
            Enabled = 2,
        }

        [RTTI.Serializable(0xF5B64B51B14862C9, 0x4)]
        public enum EDischargeMethod : int32
        {
            Timed_discharge = 0,
            Dissipate_charge__interruptible_ = 1,
            Force_dissipate__non_interruptible_ = 2,
            Instant_reset = 3,
        }

        [RTTI.Serializable(0xCF86FAD530E2F299, 0x4)]
        public enum EDiscoveryState : int32
        {
            Completed = 3,
            Discovered = 2,
            Indicated = 1,
            Undiscovered = 0,
        }

        [RTTI.Serializable(0x397BF1974C04B6C2, 0x1)]
        public enum EDisplayStatType : int8
        {
            DamageEffectiveness = 0,
            TearEffectiveness = 1,
            FireEffectiveness = 2,
            CryoEffectiveness = 3,
            ShockEffectiveness = 4,
            PoisonEffectiveness = 5,
            RangedDamageResistance = 6,
            MeleeDamageResistance = 7,
            FireResistance = 8,
            CryoResistance = 9,
            ShockResistance = 10,
            PoisonResistance = 11,
            Stealth = 12,
            Handling = 13,
        }

        [RTTI.Serializable(0xE12E702D19B35E80, 0x4)]
        public enum EDrawPartType : int32
        {
            Normal = 0,
            ShadowCasterOnly = 1,
        }

        [RTTI.Serializable(0x257405E9739AD01B, 0x4)]
        public enum EElementAutoRotate : int32
        {
            None = 0,
            RotateToLight = 1,
            RotateToCentre = 2,
        }

        [RTTI.Serializable(0x594D45A8955FBC3D, 0x4)]
        public enum EElementColor : int32
        {
            GlobalColor = 0,
            CustomColor = 1,
            Spectrum = 2,
            Gradient = 3,
        }

        [RTTI.Serializable(0xC6E5AFDFA3BBC1BE, 0x4)]
        public enum EElementTranslation : int32
        {
            None = 0,
            Free = 1,
            HorizontalOnly = 2,
            VerticalOnly = 3,
            Custom = 4,
        }

        [RTTI.Serializable(0xDCB947D4613A5490, 0x4)]
        public enum EEmitAxis : int32
        {
            x = 0,
            y = 1,
            z = 2,
        }

        [RTTI.Serializable(0x3DAF1E4DEAFB7AD, 0x4)]
        public enum EEmitterShape : int32
        {
            Sphere = 0,
            Box = 1,
            Mesh = 2,
        }

        [RTTI.Serializable(0x9D15D46468DDF141, 0x1)]
        public enum EEntityComponentSetMode : int8
        {
            Add_All_Components = 0,
            Add_Single_Component__Random_ = 1,
        }

        [RTTI.Serializable(0xD2A0C3CE20B25D2F, 0x1)]
        public enum EEntityImpostorDirection : int8
        {
            Forward = 0,
            Backward = 1,
        }

        [RTTI.Serializable(0x175D4144AEAFBADB, 0x1)]
        public enum EEntityImpostorType : int8
        {
            None = 0,
            Normal = 1,
        }

        [RTTI.Serializable(0x40E3BA087260C24, 0x1)]
        public enum EEntityLifetimeType : int8
        {
            Manual = 0,
            OtherEntity = 1,
            Scene = 2,
            Dispensable = 3,
        }

        [RTTI.Serializable(0x25F5DD98E64B3691, 0x4)]
        public enum EEntityReelType : int32
        {
            Player_To_Entity = 0,
            Entity_To_Player = 1,
        }

        [RTTI.Serializable(0x28D5473E5E578D64, 0x4)]
        public enum EEntityUpdateFrequency : int32
        {
            _60Hz = 60,
            _30Hz = 30,
            _15Hz = 15,
            _10Hz = 10,
        }

        [RTTI.Serializable(0xCC7ECB1462E6D993, 0x1)]
        public enum EEquipSlotType : int8
        {
            Invalid = 0,
            RangedWeapon = 1,
            MeleeWeapon = 2,
            HeavyWeapon = 3,
            UsableItem = 4,
            Outfit = 5,
            Uncategorized = 6,
        }

        [RTTI.Serializable(0x50A0614714C09A5A, 0x1)]
        public enum EEquipmentModificationItemCategory : int8
        {
            Weapon = 0,
            Outfit = 1,
        }

        [RTTI.Serializable(0x196A3F158648C28, 0x1)]
        public enum EExertionAnimationEventTriggerType : int8
        {
            Trigger_at_start = 0,
            Trigger_continuous = 1,
            Trigger_on_stop = 2,
        }

        [RTTI.Serializable(0x19A33500F3CF36C, 0x4)]
        public enum EExposedCombatSituationSummary : int32
        {
            invalid = 0,
            relaxed = 1,
            suspicious = 2,
            identified_unknown = 3,
            identified_observed = 4,
        }

        [RTTI.Serializable(0x53E3E207CD5A1BE9, 0x4)]
        public enum EFacialExpression : int32
        {
            None = -1,
            Default_Combat = 20,
            Anger = 0,
            Skeptical = 1,
            Exhausted = 2,
            Fear = 3,
            Nervous = 4,
            Pain = 5,
            Fire_Light = 6,
            Fire_Medium = 7,
            Fire_Heavy = 8,
            Fire_Accurate = 9,
            Fire_Throwable = 10,
            Wounded = 11,
            Revive_Instigator = 12,
            Talk_Calm = 14,
            Talk_Combat = 15,
            Talk_Player = 16,
            Hit_Response_High = 17,
            Hit_Response_Medium = 18,
            Hit_Response_Low = 19,
        }

        [RTTI.Serializable(0xCCF71554DB86462, 0x1)]
        public enum EFactContextLifetime : int8
        {
            Discard = 0,
            Persistent = 1,
        }

        [RTTI.Serializable(0x14E7D9B661359CB, 0x1)]
        public enum EFactType : int8
        {
            Unknown = 0,
            Global = 1,
            Entity = 2,
            PerkSet = 3,
            Scene = 4,
            SequenceNetwork = 5,
            Collectables = 6,
            FocusTarget = 7,
            Contextual = 8,
        }

        [RTTI.Serializable(0xEDD80B4B1AC3F838, 0x4)]
        public enum EFaction : int32
        {
            FactionNone = 0,
            FactionISA = 1,
            FactionHGH = 2,
        }

        [RTTI.Serializable(0xEBC0B502B0A56159, 0x4)]
        public enum EFactionSetMode : int32
        {
            Name = 0,
            DefaultFaction = 1,
            NeutralFaction = 2,
        }

        [RTTI.Serializable(0x9348F9FA0FAF3B84, 0x4)]
        public enum EFalloffType : int32
        {
            No = 0,
            Linear = 1,
            Square = 2,
        }

        [RTTI.Serializable(0x32EBD43E5A14AA4, 0x4)]
        public enum EFloatFactComparisonLogic : int32
        {
            Less = 0,
            LessOrEqual = 1,
            Equal = 2,
            GreaterOrEqual = 3,
            Greater = 4,
        }

        [RTTI.Serializable(0x59A2BD2E0762DAC0, 0x4)]
        public enum EFloating : int32
        {
            _0 = 0,
            left = 1,
            right = 2,
            center = 3,
        }

        [RTTI.Serializable(0x57FA319DB51894FD, 0x1)]
        public enum EFloorNrDirection : int8
        {
            Upwards = 0,
            Downwards = 1,
        }

        [RTTI.Serializable(0xAADA18796DC46CC7, 0x4)]
        public enum EFloorSlopeDetectionMethod : int32
        {
            InaccurateNormalBased = 0,
            Probes = 1,
        }

        [RTTI.Serializable(0x5FA1125ECCB513E4, 0x4)]
        public enum EFocusState : int32
        {
            Deactivated = 0,
            Deactivating = 0,
            Activated = 4,
            Scanning = 5,
            Scanned = 6,
            ScanningPart = 7,
        }

        [RTTI.Serializable(0x74A4D3E12762F39E, 0x1)]
        public enum EFocusTargetOutlineType : int8
        {
            None = 0,
            Value1 = 1,
            Value2 = 2,
            Value3 = 3,
            Value4 = 4,
            Value5 = 5,
            Value6 = 6,
        }

        [RTTI.Serializable(0xFB765B48A78E7410, 0x4)]
        public enum EForceBehaviour : int32
        {
            Vortex = 0,
            Attract = 1,
            Repel = 2,
            Push_Through = 3,
            Turbulence = 4,
            Push_Attract = 5,
            Direction_Map = 6,
        }

        [RTTI.Serializable(0xC98AC989525B4D75, 0x4)]
        public enum EForceFieldFilter : int32
        {
            All = 0,
            ForceFieldSamplerOnly = 1,
        }

        [RTTI.Serializable(0x7D6DDD43E884288D, 0x4)]
        public enum EForceFieldFlowDriver : int32
        {
            None = 0,
            Wind_speed = 1,
            Wind_speed_and_direction = 2,
            Directional_wind_speed = 3,
            Bidirectional_wind_speed = 4,
        }

        [RTTI.Serializable(0x20D55B62489AE253, 0x4)]
        public enum EForceFieldShape : int32
        {
            Sphere = 0,
            Box = 1,
        }

        [RTTI.Serializable(0x4DAB5FB9899D67FF, 0x4)]
        public enum EForceType : int32
        {
            Flow = 0,
            Force = 1,
            WaterFlow = 2,
        }

        [RTTI.Serializable(0x71429B56E7FC868D, 0x4)]
        public enum EForcedToggleType : int32
        {
            Forced_On = 0,
            Forced_Off = 1,
            Manual_Toggle = 2,
        }

        [RTTI.Serializable(0x52E00DD0CFE0AF92, 0x4)]
        public enum EForwardShadowCastMode : int32
        {
            Auto = 0,
            Enable = 1,
            Disable = 2,
        }

        [RTTI.Serializable(0x9220FF2B5D054C86, 0x4)]
        public enum EGameFunctionGroup : int32
        {
            Invalid = -1,
            Sprint = 0,
            Interact = 1,
            DodgeRoll = 2,
            Slide = 3,
            Jump = 4,
            ToggleFocus = 5,
            ShowHud = 6,
            OpenMenu = 7,
            OptionMenu = 8,
            LightMelee = 9,
            HeavyMelee = 10,
            AimWeapon = 11,
            FireWeapon = 12,
            Reload = 13,
            WeaponWhell = 14,
            Trap = 15,
            CycleToolsLeft = 16,
            CycleToolsRight = 17,
            UseMedicinePouch = 18,
            InventoryAmmoCraft = 19,
            GeneralConfirm = 20,
            MForward = 21,
            MBackward = 22,
            MLeft = 23,
            MRight = 24,
            SpeedChange = 25,
            FastEquipWeapon1 = 27,
            FastEquipWeapon2 = 28,
            FastEquipWeapon3 = 29,
            FastEquipWeapon4 = 30,
            Skills = 31,
            Inventory = 32,
            CraftMenu = 33,
            Map = 34,
            Quests = 35,
            Notebook = 36,
            HeavyAttackToggle = 26,
            GrabLeftHand = 37,
            GrabRightHand = 38,
            LockAim = 39,
            ProtoButton1 = 40,
            ProtoButton2 = 41,
            ManualSave = 42,
            TagTarget = 43,
            PageLeft = 44,
            PageRight = 45,
            TakeAll = 46,
            InGameMenuTriangle = 47,
            InGameMenuSquare = 48,
            InGameMenuCross = 49,
            JumpPC = 50,
            SkipSquence = 51,
            FastSwapWeaponUp = 52,
            FastSwapWeaponDown = 53,
            Concentration = 54,
            NumFunction = 55,
        }

        [RTTI.Serializable(0x3ED1F0C95CA59086, 0x4)]
        public enum EGameMode : int32
        {
            _0 = -1,
            Campaign = 0,
            Warzone = 1,
            Skirmish = 2,
            Coop = 3,
        }

        [RTTI.Serializable(0x5F8C69817728DB66, 0x1)]
        public enum EGameStatisticShowState : int8
        {
            AlwaysShow = 0,
            OnlyShowProgressed = 1,
            ShowProgressedAndUnknownWhenNotProgressed = 2,
        }

        [RTTI.Serializable(0xFD9374588C6E65F1, 0x1)]
        public enum EGender : int8
        {
            Male = 1,
            Female = 2,
        }

        [RTTI.Serializable(0xBAB609E6B6EE7B38, 0x4)]
        public enum EGestureBodyParts : int32
        {
            HEAD_AND_LEFT_HAND = 0,
            HEAD_AND_RIGHT_HAND = 1,
            FULLBODY_LEFT = 2,
            FULLBODY_RIGHT = 3,
        }

        [RTTI.Serializable(0x56EFC004F8653677, 0x4)]
        public enum EGestureDirection : int32
        {
            None = 0,
            Subject = 2,
            Target = 1,
        }

        [RTTI.Serializable(0x3F383A0B442542C7, 0x4)]
        public enum EGraphSoundUpdateRate : int32
        {
            Every_Synth_Frame = 1,
            Every_2nd_Synth_Frame = 2,
            Every_3rd_Synth_Frame = 3,
            Every_4th_Synth_Frame = 4,
            Every_8th_Synth_Frame = 8,
            Every_16th_Synth_Frame = 16,
        }

        [RTTI.Serializable(0x7879E52D64F1F488, 0x4)]
        public enum EGraphicsPreset : int32
        {
            Ultra = 3,
            High = 2,
            Medium = 1,
            Low = 0,
            Custom = 4,
        }

        [RTTI.Serializable(0x5175D0F0C0784E56, 0x1)]
        public enum EHAlign : int8
        {
            Default = 0,
            Left = 1,
            Center = 2,
            Right = 3,
        }

        [RTTI.Serializable(0x7BBFECEEA6FF83F9, 0x4)]
        public enum EHTTPRequestMethod : int32
        {
            GET = 1,
            POST = 2,
            PUT = 3,
        }

        [RTTI.Serializable(0x74E5A5B1C2D5E910, 0x1)]
        public enum EHUDDeviceShowOption : int8
        {
            Always = 0,
            OnlyDynamic = 1,
        }

        [RTTI.Serializable(0x7232ECA4F60CEDA0, 0x4)]
        public enum EHUDImageMode : int32
        {
            Stretch = 0,
            Tile = 1,
            AutoSize = 2,
            AspectRatioPreserved = 3,
        }

        [RTTI.Serializable(0x4F409F19512A2756, 0x4)]
        public enum EHUDLayer : int32
        {
            PostMenu = 1,
        }

        [RTTI.Serializable(0x501D02F2AA56E0A2, 0x4)]
        public enum EHUDLogicElementExpanderAxes : int32
        {
            HorizontalOnly = 0,
            VerticalOnly = 1,
            BothSimultaneously = 2,
        }

        [RTTI.Serializable(0x6BA66C6848E372F, 0x4)]
        public enum EHUDLogicElementExpanderPivot : int32
        {
            TopLeft = 0,
            TopRight = 1,
            BottomRight = 2,
            BottomLeft = 3,
            Center = 4,
        }

        [RTTI.Serializable(0xD48291BB9393F8E3, 0x4)]
        public enum EHUDLogicElementFaderMode : int32
        {
            FadeIn = 0,
            FadeOut = 1,
        }

        [RTTI.Serializable(0xE8DF7F13B281EAD0, 0x1)]
        public enum EHUDShowOption : int8
        {
            Dynamic = 0,
            AlwaysOn = 1,
            AlwaysOff = 2,
            FocusOnly = 4,
        }

        [RTTI.Serializable(0x5448AC7401A2C2D0, 0x4)]
        public enum EHUDSnap : int32
        {
            Upper_Left = 0,
            Upper_Right = 1,
            Lower_Left = 2,
            Lower_Right = 3,
            Center_Screen = 4,
            Center_Top = 5,
            Center_Bottom = 6,
            Center_Left = 7,
            Center_Right = 8,
        }

        [RTTI.Serializable(0x5FDAD4EE39321DD8, 0x4)]
        public enum EHUDTextMode : int32
        {
            Unclipped = 0,
            AutoSize = 1,
            WordWrap = 2,
            WordWrapAutoSize = 3,
            AutoFitTextSize = 4,
        }

        [RTTI.Serializable(0xB2D622196081F405, 0x4)]
        public enum EHUDUnits : int32
        {
            Pixels = 0,
            Percentage = 1,
        }

        [RTTI.Serializable(0xECC88F51D21C6554, 0x4)]
        public enum EHealthRegenerationSettings : int32
        {
            Slow = 0,
            Normal = 1,
            Fast = 2,
        }

        [RTTI.Serializable(0x3D93D5F3E83CBF2A, 0x4)]
        public enum EHeightChannel : int32
        {
            HeightNone = 4,
            Height_Red = 0,
            Height_Green = 1,
            Height_Blue = 2,
            Height_Alpha = 3,
        }

        [RTTI.Serializable(0x8CDE1C075D1CD118, 0x1)]
        public enum EHierarchyLevelToReassignTo : int8
        {
            same_group = 0,
            one_level_up = 1,
            two_levels_up = 2,
            three_levels_up = 3,
            four_levels_up = 4,
            five_levels_up = 5,
        }

        [RTTI.Serializable(0x4151F1C68847603B, 0x4)]
        public enum EHitDirection : int32
        {
            Any = 0,
            Back = 1,
            Left = 2,
            Front = 3,
            Right = 4,
        }

        [RTTI.Serializable(0x441E5FF654CAFE3F, 0x4)]
        public enum EHitLocation : int32
        {
            Any = 0,
            Head = 1,
            Chest = 2,
            Stomach = 3,
            RightArm = 4,
            RightLowerArm = 5,
            LeftArm = 6,
            LeftLowerArm = 7,
            RightLeg = 8,
            LeftLeg = 9,
            User1 = 10,
            User2 = 11,
        }

        [RTTI.Serializable(0x9DEFFE596FE7BD79, 0x4)]
        public enum EHitReactionAccumulationType : int32
        {
            Impact_Severity = 0,
            Damage = 1,
        }

        [RTTI.Serializable(0x8983DEBA88F73304, 0x4)]
        public enum EHitReactionCycleMode : int32
        {
            Cycle = 0,
            Reset_Last = 1,
            Disable = 2,
        }

        [RTTI.Serializable(0x4C8104B2E2CDA797, 0x4)]
        public enum EHitResponse : int32
        {
            None = 0,
            PhysicsOnly = 1,
            InPlace = 2,
            Full = 3,
        }

        [RTTI.Serializable(0x9A6263284BC5DB20, 0x4)]
        public enum EHitResponseType : int32
        {
            Twitch = 0,
            Flinch = 1,
            Stagger = 2,
            Knockback = 3,
            AnimatedKnockdown = 4,
            RagdollKnockdown = 5,
            AnimatedDeath = 6,
            RagdollDeath = 7,
            CinematicDeath = 8,
        }

        [RTTI.Serializable(0x9D4D659E045A7500, 0x4)]
        public enum EHomeBaseVisibility : int32
        {
            CarrierOnly = 0,
            CarrierFaction = 1,
            All = 2,
        }

        [RTTI.Serializable(0x34985EA95C54BD9A, 0x1)]
        public enum EHorseRestoreState : int8
        {
            NoHorse = 0,
            HasHorse = 1,
            HasHorseAndIsMounted = 2,
        }

        [RTTI.Serializable(0xFDC063DCFC3A24C3, 0x4)]
        public enum EHumanoidDismountMovement : int32
        {
            Idle = 0,
            Moving = 1,
            Falling = 2,
            Dead = 3,
        }

        [RTTI.Serializable(0x1E39615F7DE84BF, 0x4)]
        public enum EIconType : int32
        {
            Invalid = -1,
            Right = 13,
            Left = 15,
            Up = 12,
            Down = 14,
            Triangle = 0,
            Circle = 1,
            Cross = 2,
            Square = 3,
            Shoulder_Left_1 = 6,
            Shoulder_Left_2 = 4,
            Shoulder_Right_1 = 7,
            Shoulder_Right_2 = 5,
            Left_Analog_Button = 10,
            Right_Analog_Button = 11,
            Select = 9,
            Start = 8,
            Touch_Pad = 21,
            Touch_Pad_Left = 22,
            Touch_Pad_Right = 23,
            Left_Analog = 194,
            Right_Analog = 195,
            Xbox_None = 30,
            Xbox_Menu = 31,
            Xbox_View = 32,
            Xbox_A = 33,
            Xbox_B = 34,
            Xbox_X = 35,
            Xbox_Y = 36,
            Xbox_Dpad_Up = 37,
            Xbox_Dpad_Down = 38,
            Xbox_Dpad_Left = 39,
            Xbox_Dpad_Right = 40,
            Xbox_Left_Shoulder = 41,
            Xbox_Right_Shoulder = 42,
            Xbox_Left_Trigger = 43,
            Xbox_Right_Trigger = 44,
            Xbox_Left_Thumbstick_Button = 45,
            Xbox_Right_Thumbstick_Button = 46,
            Xbox_Left_Thumbstick = 196,
            Xbox_Right_Thumbstick = 197,
            Steam_None = 47,
            Steam_Start = 48,
            Steam_Select = 49,
            Steam_A = 50,
            Steam_B = 51,
            Steam_X = 52,
            Steam_Y = 53,
            Steam_Lpad_Up = 54,
            Steam_Lpad_Down = 55,
            Steam_Lpad_Left = 56,
            Steam_Lpad_Right = 57,
            Steam_Left_Bumper = 58,
            Steam_Right_Bumper = 59,
            Steam_Left_Trigger = 60,
            Steam_Right_Trigger = 61,
            Steam_Left_Thumbstick_button = 62,
            Steam_Right_Pad_button = 63,
            Steam_Left_Thumbstick = 198,
            Steam_Right_Pad = 199,
            Steam_Right_BackPanel = 64,
            Steam_Left_BackPanel = 65,
            Key_None = 66,
            Key_Esc = 67,
            Key_Plus = 68,
            Key_Minus = 69,
            Key_Space = 70,
            Key_Oquote = 71,
            Key_Cquote = 72,
            Key_Lhook = 73,
            Key_Rhook = 74,
            Key_Bslash = 75,
            Key_Fslash = 76,
            Key_Semicolon = 77,
            Key_Comma = 78,
            Key_Dot = 79,
            Key_Enter = 80,
            Key_Backspace = 81,
            Key_Tab = 82,
            Key_Left = 83,
            Key_Right = 84,
            Key_Up = 85,
            Key_Down = 86,
            Key_Home = 87,
            Key_End = 88,
            Key_Pgup = 89,
            Key_Pgdn = 90,
            Key_Ins = 91,
            Key_Del = 92,
            Key_Pad_Mul = 93,
            Key_Pad_Div = 94,
            Key_Pad_Plus = 95,
            Key_Pad_Minus = 96,
            Key_Pad_Enter = 97,
            Key_Pad_0 = 98,
            Key_Pad_1 = 99,
            Key_Pad_2 = 100,
            Key_Pad_3 = 101,
            Key_Pad_4 = 102,
            Key_Pad_5 = 103,
            Key_Pad_6 = 104,
            Key_Pad_7 = 105,
            Key_Pad_8 = 106,
            Key_Pad_9 = 107,
            Key_Pad_Del = 108,
            Key_Capslock = 109,
            Key_Printscreen = 110,
            Key_Scrolllock = 111,
            Key_Numlock = 112,
            Key_Pause = 113,
            Key_Lalt = 114,
            Key_Ralt = 115,
            Key_Lctrl = 116,
            Key_Rctrl = 117,
            Key_Lshift = 118,
            Key_Rshift = 119,
            Key_Win_Lwinkey = 120,
            Key_Win_Rwinkey = 121,
            Key_Win_Context = 122,
            Key_F1 = 123,
            Key_F2 = 124,
            Key_F3 = 125,
            Key_F4 = 126,
            Key_F5 = 127,
            Key_F6 = 128,
            Key_F7 = 129,
            Key_F8 = 130,
            Key_F9 = 131,
            Key_F10 = 132,
            Key_F11 = 133,
            Key_F12 = 134,
            Key_0 = 135,
            Key_1 = 136,
            Key_2 = 137,
            Key_3 = 138,
            Key_4 = 139,
            Key_5 = 140,
            Key_6 = 141,
            Key_7 = 142,
            Key_8 = 143,
            Key_9 = 144,
            Key_A = 145,
            Key_B = 146,
            Key_C = 147,
            Key_D = 148,
            Key_E = 149,
            Key_F = 150,
            Key_G = 151,
            Key_H = 152,
            Key_I = 153,
            Key_J = 154,
            Key_K = 155,
            Key_L = 156,
            Key_M = 157,
            Key_N = 158,
            Key_O = 159,
            Key_P = 160,
            Key_Q = 161,
            Key_R = 162,
            Key_S = 163,
            Key_T = 164,
            Key_U = 165,
            Key_V = 166,
            Key_W = 167,
            Key_X = 168,
            Key_Y = 169,
            Key_Z = 170,
            KEY_SHARP = 171,
            KEY_OEM_1 = 172,
            KEY_OEM_2 = 173,
            KEY_OEM_3 = 174,
            KEY_OEM_4 = 175,
            KEY_OEM_5 = 176,
            KEY_OEM_6 = 177,
            KEY_OEM_7 = 178,
            KEY_OEM_8 = 179,
            KEY_OEM_102 = 180,
            Key_Caps_Toggle = 181,
            Key_Num_Toggle = 182,
            Key_Scroll_Toggle = 183,
            Key_Enter_Extend = 184,
            Key_Esc_Extend = 185,
            Mouse_Left = 187,
            Mouse_Right = 188,
            Mouse_Middle = 189,
            Mouse_XButton1 = 190,
            Mouse_XButton2 = 191,
            Mouse_No_Click = 200,
            Virtual_Mouse_Left = 186,
        }

        [RTTI.Serializable(0xF2A372102AA8AE6A, 0x4)]
        public enum EImageCompressionMethod : int32
        {
            PerceptualData = 0,
            NormalData = 1,
            VariableData = 2,
            DefaultData = 3,
        }

        [RTTI.Serializable(0xCA564630080B732D, 0x4)]
        public enum EImpactEffectOverrideMode : int32
        {
            Add = 0,
            Replace = 1,
        }

        [RTTI.Serializable(0x15F24152ACDFBE11, 0x4)]
        public enum EIndirectLightingHint : int32
        {
            Default = 0,
            UndergroundVolume = 1,
        }

        [RTTI.Serializable(0xF13C0631AB8D0FC5, 0x4)]
        public enum EIntegerFactComparisonLogic : int32
        {
            Less = 0,
            LessOrEqual = 1,
            Equal = 2,
            GreaterOrEqual = 3,
            Greater = 4,
        }

        [RTTI.Serializable(0x4084E227EDD0424D, 0x4)]
        public enum EIntersectionMethod : int32
        {
            Line_Intersection = 0,
            Swept_Sphere_Intersection = 1,
        }

        [RTTI.Serializable(0x8248FC6A315A5C15, 0x1)]
        public enum EInventoryCategory : int8
        {
            Weapons = 0,
            Tools = 1,
            Ammo = 2,
            Modifications = 3,
            Outfits = 4,
            Resources = 5,
            Special = 6,
            LootBoxes = 7,
            NumCategories = 8,
            None = 9,
        }

        [RTTI.Serializable(0x39C4705BB59248AF, 0x1)]
        public enum EInventoryItemAddType : int8
        {
            Regular = 0,
            IgnoreCapacity = 1,
            Transfer = 2,
            LoadSave = 3,
            Craft = 4,
            Merchant = 5,
            Remember = 6,
            BuyBack = 7,
        }

        [RTTI.Serializable(0x7FF809DD0DB7948E, 0x1)]
        public enum EInventoryItemRemoveType : int8
        {
            Destroy = 0,
            Transfer = 1,
            Drop = 2,
            Craft = 3,
            Consume = 4,
            Remember = 5,
        }

        [RTTI.Serializable(0xA4BE78E9198464A7, 0x4)]
        public enum ELanguage : int32
        {
            English = 1,
            Unknown = 0,
            Dutch = 6,
            German = 4,
            French = 2,
            Spanish = 3,
            Italian = 5,
            Portugese = 7,
            Japanese = 16,
            Chinese_Traditional = 8,
            Korean = 9,
            Russian = 10,
            Polish = 11,
            Danish = 12,
            Finnish = 13,
            Norwegian = 14,
            Swedish = 15,
            LATAMSP = 17,
            LATAMPOR = 18,
            Turkish = 19,
            Arabic = 20,
            Chinese_Simplified = 21,
        }

        [RTTI.Serializable(0xD1DEA84A604CCF6E, 0x4)]
        public enum ELayerBlendOperation : int32
        {
            Invalid = 0,
            AlphaBlend = 1,
            IndexBlend = 2,
            Multiply = 3,
            Add = 4,
            Subtract = 5,
            Max = 6,
        }

        [RTTI.Serializable(0x8443BFF5D34450CA, 0x4)]
        public enum ELayerGroupingMask : int32
        {
            None = 0,
            AccumulativeGroup = 1,
            DensityGroup = 2,
            CompositeGroup = 4,
            BakeGroup = 8,
        }

        [RTTI.Serializable(0x9624952CE3A21EE9, 0x4)]
        public enum ELayerOperation : int32
        {
            Invalid = -1,
            Write = 0,
            Mul = 1,
            Mul2x = 2,
            Add = 3,
            Sub = 4,
            Min = 12,
            Max = 13,
            AlphaBlend = 5,
            AlphaMul = 6,
            AlphaMul2x = 7,
            AlphaAdd = 8,
            AlphaSub = 9,
            InvAlphaMul = 10,
            PreMulAlphaBlend = 11,
            WriteMask = 14,
            MaskedWrite = 15,
            MaskedAdd = 16,
            MaskedSub = 17,
            MulInvSrcColor = 18,
        }

        [RTTI.Serializable(0x8858CC7255D55071, 0x4)]
        public enum ELegendButton : int32
        {
            up = 0,
            down = 1,
            left = 2,
            right = 3,
            accept = 4,
            cancel = 5,
            inbox = 6,
            options = 7,
            intel = 8,
            start = 9,
            tableft = 10,
            tabright = 11,
            cycleprev = 12,
            cyclenext = 13,
            leftstick = 14,
            rightstick = 15,
            leftstickpress = 16,
            rightstickpress = 17,
            tabrightapply = 18,
            tabrighttravel = 19,
            runbenchmarkpress = 20,
            acceptextend = 21,
            cancelextend = 22,
            ingamemenucross = 24,
            skipmovie = 27,
            loottakeall = 23,
            ingamemenutriangle = 25,
            ingamemenusquare = 26,
            takeshot = 28,
            maptravel = 29,
            mapactivequest = 30,
            mapgoindoor = 31,
            mapdownlowerfloor = 32,
            mapuphigherfloor = 33,
            mapwaypoint = 34,
        }

        [RTTI.Serializable(0x58A67CBC959845BA, 0x4)]
        public enum ELensFlareTriggerFalloff : int32
        {
            Linear = 0,
            Smooth = 1,
            Exponential = 2,
        }

        [RTTI.Serializable(0xDDA988904A7C57F3, 0x4)]
        public enum ELensFlareTriggerMode : int32
        {
            ObjectPosition = 0,
            LightPosition = 1,
        }

        [RTTI.Serializable(0xDF69D6395DB05C73, 0x4)]
        public enum ELensFlareTriggerType : int32
        {
            FromBorder = 0,
            FromCentre = 1,
            FromLight = 2,
        }

        [RTTI.Serializable(0x7D4B500EDF014423, 0x4)]
        public enum ELightAreaType : int32
        {
            Point = 0,
            Disk = 1,
            Rect = 2,
        }

        [RTTI.Serializable(0xDF730EA39C15E444, 0x4)]
        public enum ELightCollectionIdentifierMode : int32
        {
            TimeOfDay = 0,
            NamedLightCollection = 1,
        }

        [RTTI.Serializable(0x6593F416DE1C45C1, 0x4)]
        public enum ELightInfluenceAccuracy : int32
        {
            Auto = 0,
            Precise = 1,
            Fast = 2,
        }

        [RTTI.Serializable(0x430CD8E9A1330C8C, 0x4)]
        public enum ELightProbeSetType : int32
        {
            Master = 0,
            Subset = 1,
        }

        [RTTI.Serializable(0xC6A69AFA7AD2A016, 0x4)]
        public enum ELightSamplingResolution : int32
        {
            LightSamplingRes8x8 = 3,
            LightSamplingRes4x4 = 2,
            LightSamplingRes2x2 = 1,
            LightSamplingRes1x1 = 0,
        }

        [RTTI.Serializable(0x6B1CC97EFF088A9, 0x4)]
        public enum ELightbakeZoneOrientation : int32
        {
            WorldSpace = 0,
            BakeZoneSpace = 1,
        }

        [RTTI.Serializable(0xEED1740D649A6608, 0x4)]
        public enum ELightbakeZoneQuality : int32
        {
            Default = 0,
            High = 1,
        }

        [RTTI.Serializable(0xC45E86399F103111, 0x4)]
        public enum ELightbakeZoneRestriction : int32
        {
            AboveGround = 1,
            BelowGround = 0,
            Universal = 2,
            GroundLevel = 3,
        }

        [RTTI.Serializable(0xFE68E268F2C88D0D, 0x4)]
        public enum ELightmapEncodeColorScale : int32
        {
            Do_not_scale = 0,
            Scale_so_one_pixel_in_100_is_clamped__10x10_ = 2,
            Scale_so_one_pixel_in_300_is_clamped__15x15_ = 3,
            Scale_so_one_pixel_in_1000_is_clamped__30x30_ = 4,
            Scale_so_one_pixel_in_3000_is_clamped__50x50_ = 5,
            Scale_so_one_pixel_in_10000_is_clamped__100x100_ = 6,
            Scale_by_brightest_color = 1,
        }

        [RTTI.Serializable(0x5B8BDA3CA6E0D9B0, 0x4)]
        public enum ELocationType : int32
        {
            Global = 0,
            Local = 1,
        }

        [RTTI.Serializable(0xEE0F852A08DC1682, 0x4)]
        public enum ELookDirection : int32
        {
            None = 0,
            Subject = 2,
            Target = 1,
        }

        [RTTI.Serializable(0xE7762EF7D7C7B274, 0x4)]
        public enum ELoopMode : int32
        {
            Off = 0,
            On = 1,
            Hold = 2,
            PingPong = 3,
        }

        [RTTI.Serializable(0x7D601D08618768E6, 0x1)]
        public enum ELootDataIncrementType : int8
        {
            LootSlotLevel = 0,
            LootDataLevel = 1,
        }

        [RTTI.Serializable(0x8446E575289738C7, 0x1)]
        public enum ELootItemCategory : int8
        {
            MachineHeart = 7,
            MachineLens = 8,
            AnimalSkin = 11,
            AnimalBone = 12,
            Wood = 16,
            FastTravel = 13,
            MachineResource = 6,
            AnimalResource = 4,
            NaturalResource = 3,
            Glass = 1,
            Token = 14,
            OldWorldItem = 0,
            MachineCore = 9,
            AnimalTalisman = 5,
            MachineElement = 10,
            Other = 15,
            Junk = 2,
        }

        [RTTI.Serializable(0x9F2FEF971851C17A, 0x4)]
        public enum ELootItemRarity : int32
        {
            Common = 0,
            Uncommon = 1,
            Rare = 2,
            VeryRare = 3,
        }

        [RTTI.Serializable(0x67B73BEC3200B74C, 0x1)]
        public enum EMapZoneRevealAreaMode : int8
        {
            PlayerOnly = 0,
            MapOnly = 1,
            PlayerAndMap = 2,
        }

        [RTTI.Serializable(0x68B1263E2BE0C061, 0x1)]
        public enum EMapZoomLevel : int8
        {
            LowZoom = 0,
            MediumZoom = 1,
            HighZoom = 2,
        }

        [RTTI.Serializable(0x154601C70618BC72, 0x4)]
        public enum EMaskChannel : int32
        {
            MaskNone = 16,
            Mask_A_Red = 0,
            Mask_A_Green = 1,
            Mask_A_Blue = 2,
            Mask_A_Alpha = 3,
            Mask_B_Red = 4,
            Mask_B_Green = 5,
            Mask_B_Blue = 6,
            Mask_B_Alpha = 7,
            Mask_C_Red = 8,
            Mask_C_Green = 9,
            Mask_C_Blue = 10,
            Mask_C_Alpha = 11,
            Mask_D_Red = 12,
            Mask_D_Green = 13,
            Mask_D_Blue = 14,
            Mask_D_Alpha = 15,
        }

        [RTTI.Serializable(0x2A745752926128EF, 0x4)]
        public enum EMaterialDebugType : int32
        {
            Static = 0,
            Dynamic = 1,
        }

        [RTTI.Serializable(0x21A790626493E43, 0x4)]
        public enum EMeleeDamageImpulseDirectionType : int32
        {
            BoxMovementDirection = 0,
            Radial = 1,
            FixedToEntity = 2,
            FixedToParentEntity = 3,
        }

        [RTTI.Serializable(0x6C8CF660D14719E2, 0x4)]
        public enum EMenuActionFocusType : int32
        {
            Target = 0,
            FirstChild = 1,
            LastChild = 2,
        }

        [RTTI.Serializable(0x56C84D3FC35B6104, 0x1)]
        public enum EMenuAnimatableProperty : int8
        {
            OffsetX = 0,
            OffsetY = 1,
            Opacity = 2,
            FontScale = 3,
            TextureScale = 4,
        }

        [RTTI.Serializable(0xE07F7F1609A3E006, 0x1)]
        public enum EMenuAnimationTrigger : int8
        {
            Idle = 0,
            FocusReceived = 2,
            FocusLost = 3,
            PageOpen = 4,
            PageLeave = 5,
            OnShow = 6,
            OnHide = 7,
            Scripted = 1,
            OnMouseHoverReceived = 8,
            OnMouseHoverLost = 9,
        }

        [RTTI.Serializable(0xEE9C2CC496FF8CD5, 0x1)]
        public enum EMenuAnimationUpdateFrequency : int8
        {
            _30Hz = 30,
            _60Hz = 60,
        }

        [RTTI.Serializable(0x34050C47CB9647B7, 0x1)]
        public enum EMenuBadgeCategory : int8
        {
            Collectables = 21,
            DLCCollectables = 24,
            CatalogueRobots = 22,
            CatalogueDataCubes = 23,
        }

        [RTTI.Serializable(0x528E239EC33CA8B3, 0x4)]
        public enum EMenuEvent : int32
        {
            Unset = -1,
            OnPressAccept = 13,
            OnPressCancel = 14,
            OnPressStart = 15,
            OnPressDpadUp = 16,
            OnPressDpadDown = 17,
            OnPressDpadLeft = 18,
            OnPressDpadRight = 19,
            OnPressUp = 20,
            OnPressDown = 21,
            OnPressLeft = 22,
            OnPressRight = 23,
            OnPressLeftAnalog = 24,
            OnPressRightAnalog = 25,
            OnInbox = 26,
            OnOptions = 27,
            OnIntel = 28,
            OnPressNextTab = 29,
            OnPressPrevTab = 30,
            OnCycleNext = 31,
            OnCyclePrev = 32,
            OnAnalogClockwise = 33,
            OnAnalogCounterClockwise = 34,
            OnPressNextTabApply = 35,
            OnPressNextTabTravel = 36,
            OnPressRunBenchmark = 37,
            OnPressAcceptExtend = 38,
            OnPressCancelExtend = 39,
            OnPressInGameButtonCross = 41,
            OnPressSkipMovie = 50,
            OnPressSkipMovieTrigger = 51,
            OnPressTakeAll = 40,
            OnPressInGameButtonTriangle = 42,
            OnPressInGameButtonSquare = 43,
            OnPressIngameSkill = 44,
            OnPressIngameInventory = 45,
            OnPressIngameCrafting = 46,
            OnPressIngameMap = 47,
            OnPressIngameQuests = 48,
            OnPressIngameNotebook = 49,
            OnPressAnykey = 74,
            OnPressAcceptHold = 76,
            OnPressCancelHold = 77,
            OnPressStartHold = 78,
            OnPressDpadUpHold = 79,
            OnPressDpadDownHold = 80,
            OnPressDpadLeftHold = 81,
            OnPressDpadRightHold = 82,
            OnPressUpHold = 83,
            OnPressDownHold = 84,
            OnPressLeftHold = 85,
            OnPressRightHold = 86,
            OnPressLeftAnalogHold = 87,
            OnPressRightAnalogHold = 88,
            OnInboxHold = 89,
            OnOptionsHold = 90,
            OnIntelHold = 91,
            OnPressNextTabHold = 92,
            OnPressPrevTabHold = 93,
            OnCycleNextHold = 94,
            OnCyclePrevHold = 95,
            OnAnalogClockwiseHold = 96,
            OnAnalogCounterClockwiseHold = 97,
            OnBindKeyFinished = 6,
            OnControllerTypeChanged = 7,
            OnUnplugMonitor = 8,
            OnWindowMoved = 9,
            OnDownloadComplete = 10,
            OnPressInGameButtonCrossHold = 98,
            OnMouseLeftHold = 101,
            OnPressInGameButtonTriangleHold = 99,
            OnPressInGameButtonSquareHold = 100,
            OnSpecailFocusMe = 71,
            OnMouseHoverIn = 72,
            OnMouseLeftDown = 60,
            OnMouseLeftClick = 64,
            OnMouseLeftClickNoFocus = 65,
            OnMouseEnter = 68,
            OnMouseLeftUp = 61,
            OnMouseMove = 69,
            OnMouseWheel = 70,
            OnFocusOn = 1,
            OnFocusOff = 2,
            OnPageOn = 3,
            OnPageOff = 4,
            OnValueChanged = 5,
            OnPressTakeShot = 52,
            OnPressMapTravel = 53,
            OnPressMapActiveQuest = 54,
            OnPressMapGoIndoor = 55,
            OnPressMapDownLowerFloor = 56,
            OnPressMapUpHigherFloor = 57,
            OnPressMapWayPoint = 58,
        }

        [RTTI.Serializable(0xB7F0AA0283428099, 0x4)]
        public enum EMenuInputFunction : int32
        {
            FUNCTION_UNSET = -1,
            FUNCTION_DPAD_NAV_UP = 0,
            FUNCTION_DPAD_NAV_DOWN = 1,
            FUNCTION_DPAD_NAV_LEFT = 2,
            FUNCTION_DPAD_NAV_RIGHT = 3,
            FUNCTION_NAV_UP = 4,
            FUNCTION_NAV_DOWN = 5,
            FUNCTION_NAV_LEFT = 6,
            FUNCTION_NAV_RIGHT = 7,
            FUNCTION_SCROLL_UP = 8,
            FUNCTION_SCROLL_DOWN = 9,
            FUNCTION_ACCEPT = 10,
            FUNCTION_OPEN_VKB = 11,
            FUNCTION_CANCEL = 12,
            FUNCTION_TAB_PREVIOUS = 13,
            FUNCTION_TAB_NEXT = 14,
            FUNCTION_CYCLE_PREVIOUS = 15,
            FUNCTION_CYCLE_NEXT = 16,
            FUNCTION_INBOX = 17,
            FUNCTION_MENU_OPTIONS = 18,
            FUNCTION_INGAME_OPTIONS = 19,
            FUNCTION_INGAME_INTEL = 20,
            FUNCTION_ANALOG_CLOCKWISE = 21,
            FUNCTION_ANALOG_COUNTERCLOCKWISE = 22,
            FUNCTION_ANALOG_RIGHT = 23,
            FUNCTION_ANALOG_LEFT = 24,
            FUNCTION_TAB_NEXT_APPLY = 25,
            FUNCTION_TAB_NEXT_TRAVEL = 26,
            FUNCTION_ANALOG_RIGHT_RUNBENCHMARK = 27,
            FUNCTION_ACCEPT_EXTEND = 28,
            FUNCTION_CANCEL_EXTEND = 29,
            FUNCTION_SKIP_MOVIE = 30,
            FUNCTION_SKIP_MOVIE_TRIGGER = 31,
            FUNCTION_MAP_TRAVEL = 32,
            FUNCTION_MAP_ACTIVE_QUEST = 33,
            FUNCTION_MAP_GO_INDOOR = 34,
            FUNCTION_MAP_DOWN_LOWER_FLOOR = 35,
            FUNCTION_MAP_UP_HEIGHER_FLOOR = 36,
            FUNCTION_MAP_WAYPOINT = 37,
            FUNCTION_LOOT_TAKE_ALL = 38,
            FUNCTION_IN_GAME_BUTTON_CROSS = 39,
            FUNCTION_IN_GAME_BUTTON_TRIANGLE = 40,
            FUNCTION_IN_GAME_BUTTON_SQUARE = 41,
            FUNCTION_TAKE_SHOT = 42,
            FUNCTION_INGAME_SKILL = 43,
            FUNCTION_INGAME_INVENTORY = 44,
            FUNCTION_INGAME_CRAFTING = 45,
            FUNCTION_INGAME_MAP = 46,
            FUNCTION_INGAME_QUESTS = 47,
            FUNCTION_INGAME_NOTEBOOK = 48,
            FUNCTION_ANYKEY = 66,
        }

        [RTTI.Serializable(0x8E5AB6BFB550B0D2, 0x1)]
        public enum EMenuInventorySortOrder : int8
        {
            Unset = 0,
            RarityAndPriceAscending = 1,
            RarityAndPriceDescending = 2,
        }

        [RTTI.Serializable(0x2B5FE94BB2C19E41, 0x1)]
        public enum EMerchantSupplySettings : int8
        {
            Infinite = 0,
            ReStockable = 1,
        }

        [RTTI.Serializable(0x8B3F7A89047F440D, 0x4)]
        public enum EMeshEmitterSpawnOrder : int32
        {
            Point_order = 0,
            Random_order = 1,
        }

        [RTTI.Serializable(0x5026FC3B82F04D20, 0x4)]
        public enum EMineMode : int32
        {
            Pressure = 0,
            Trip_Wire = 1,
        }

        [RTTI.Serializable(0x864D66E5BF156FD0, 0x4)]
        public enum EMissionType : int32
        {
            campaign = 31,
            coop = 30,
            infiltrate_and_retrieve = 7,
            search_and_retrieve = 6,
            search_and_destroy = 5,
            search_and_safeguard = 4,
            capture_and_hold = 3,
            capture_and_connect = 2,
            capture_and_secure = 1,
            body_count = 0,
        }

        [RTTI.Serializable(0xB3A0B97363002E13, 0x1)]
        public enum EModifiableStat : int8
        {
            ChargeRate = 0,
            AmmoChargeRate = 1,
            ExitVelocity = 2,
            ReloadSpeed = 3,
            WieldStowSpeed = 4,
            ChargeAccuracy = 5,
            FireRate = 6,
            EffectiveRange = 7,
            VisualStimulusSize = 8,
            FootstepStimulusLoudness = 9,
        }

        [RTTI.Serializable(0xDBA18D6B9F6EC14C, 0x4)]
        public enum EMountControlDirection : int32
        {
            MountDirectionNone = 0,
            MountDirectionRight = 1,
            MountDirectionForward = 2,
            MountDirectionLeft = 3,
            MountDirectionBackwards = 4,
            MountDirectionInvalid = 5,
        }

        [RTTI.Serializable(0x19813B25F9BA2283, 0x4)]
        public enum EMountDeviceFunction : int32
        {
            RangedAttack = 0,
            MeleeAttack = 1,
            SecondaryMeleeAttack = 2,
            Invalid = 3,
        }

        [RTTI.Serializable(0xE96B6AECBB0BB9F6, 0x4)]
        public enum EMountDismountLocation : int32
        {
            INVALID = -1,
            FRONT = 0,
            LEFT = 1,
            BACK = 2,
            RIGHT = 3,
            CORNER_LEFT = 4,
            CORNER_RIGHT = 5,
            STEP_OUT = 6,
            BACK_LEFT = 7,
            BACK_RIGHT = 8,
        }

        [RTTI.Serializable(0xFC9C4B51F53EE145, 0x4)]
        public enum EMountMovementState : int32
        {
            MountMovementStopped = 0,
            MountMovementWalking = 1,
            MountMovementInvalid = 2,
        }

        [RTTI.Serializable(0x30CB9FF9A4E7DC81, 0x1)]
        public enum EMountRequest : int8
        {
            Mount = 0,
            Dismount = 1,
        }

        [RTTI.Serializable(0x7764D72164C08F4F, 0x1)]
        public enum EMountState : int8
        {
            Unmounted = 3,
            Mounting = 0,
            Mounted = 1,
            Dismounting = 2,
        }

        [RTTI.Serializable(0xD51C0F39D428565B, 0x4)]
        public enum EMountedState : int32
        {
            Any = 0,
            Unmounted = 1,
            Mounted = 2,
        }

        [RTTI.Serializable(0x2839A5DF088DABFD, 0x4)]
        public enum EMoveStanceChoice : int32
        {
            Fast = 0,
            Stealth = 1,
        }

        [RTTI.Serializable(0x913848C879A8AD3B, 0x1)]
        public enum EMovementStateGroundToAir : int8
        {
            On_Ground = 0,
            Taking_Off = 1,
            Landing = 3,
            Flying = 2,
        }

        [RTTI.Serializable(0x5498ACA490AF9595, 0x4)]
        public enum EMoverActionParentLinking : int32
        {
            DontChange = 0,
            AttachToActivator = 1,
            DetachFromParent = 2,
        }

        [RTTI.Serializable(0x6FCAD7DDEB17485F, 0x4)]
        public enum EMovieFadePurpose : int32
        {
            None = 0,
            Intro = 2,
            Outro = 3,
            Taboo = 1,
        }

        [RTTI.Serializable(0x3488515A4A0C6249, 0x4)]
        public enum EMovieListType : int32
        {
            Game = 0,
            Menu = 1,
            Menu_Level = 2,
        }

        [RTTI.Serializable(0x9667CA356B15D4B7, 0x4)]
        public enum EMovieMemoryType : int32
        {
            Heap = 0,
            Post = 1,
        }

        [RTTI.Serializable(0x22DA4F3DC400014B, 0x1)]
        public enum EMsgAIAttackState : int8
        {
            Start = 0,
            Complete = 1,
            Abort = 2,
        }

        [RTTI.Serializable(0xA9CD3B8D74F4B30, 0x4)]
        public enum EMusicTransitionCurveType : int32
        {
            Linear = 1,
            Fast = 2,
            Slow = 3,
            Smooth = 4,
            Sharp = 5,
        }

        [RTTI.Serializable(0xFE3F700346F72175, 0x4)]
        public enum EMusicTransitionMode : int32
        {
            Seconds = 0,
            Beats = 1,
        }

        [RTTI.Serializable(0xC0CF16FEFD9F8DBD, 0x4)]
        public enum ENameExposureType : int32
        {
            Never = 0,
            Always = 1,
            OnTarget = 2,
        }

        [RTTI.Serializable(0x8D07B544A2A2EE07, 0x4)]
        public enum ENoProjectileTraceType : int32
        {
            Position = 0,
            Target = 1,
        }

        [RTTI.Serializable(0xC0FF13D8C15AB393, 0x4)]
        public enum EOWLMode : int32
        {
            Hover = 0,
            Flight = 1,
            Combat = 2,
            Hacking = 3,
            Shield = 4,
        }

        [RTTI.Serializable(0x6CBFDA00E9A30825, 0x4)]
        public enum EObjectiveCompleteFailLogic : int32
        {
            AnySucceedAnyFail = 0,
            AnySucceedAllFail = 1,
            AllSucceedAnyFail = 2,
        }

        [RTTI.Serializable(0x91D855361D5222B3, 0x4)]
        public enum EObjectiveUIVisibility : int32
        {
            Visible = 0,
            VisibleOnMapOnly = 1,
            VisibleInHudOnly = 2,
            VisibleInAllButTracker = 3,
            Hidden = 4,
        }

        [RTTI.Serializable(0x41A5D2EC630157FE, 0x4)]
        public enum EObstacleShape : int32
        {
            Entity_Physics = 1,
            Custom_Box = 3,
            ModelPart_Boxes = 4,
        }

        [RTTI.Serializable(0x7205DA5456ECAABA, 0x4)]
        public enum EObstacleType : int32
        {
            Normal = 1,
            Ignore = 0,
            Soft = 2,
            Hard = 3,
        }

        [RTTI.Serializable(0x58CD4BCCCEC76A0D, 0x4)]
        public enum EOnDeathDropLogic : int32
        {
            Don_t_Drop = 0,
            Immediate = 1,
            Delayed = 2,
        }

        [RTTI.Serializable(0x604598E99B42EA29, 0x4)]
        public enum EOpacityMode : int32
        {
            _0 = 0,
            inherit = 1,
            ignore = 2,
        }

        [RTTI.Serializable(0xBA1988C52E5AEA95, 0x4)]
        public enum EOperator : int32
        {
            NoOperator = 0,
            _1 = 1,
            _2 = 2,
            _3 = 3,
            _4 = 4,
            _5 = 5,
        }

        [RTTI.Serializable(0x10D64D59B8D975AE, 0x4)]
        public enum EPBDConstraintDescType : int32
        {
            Distance = 1,
            DistanceLRA = 6,
            Bend = 7,
        }

        [RTTI.Serializable(0x59AB830AD2E4FAF6, 0x1)]
        public enum EPODVariantType : int8
        {
            Invalid = 0,
            Integer = 1,
            Integer8 = 2,
            UnsignedInteger = 3,
            UnsignedInteger8 = 4,
            Float = 5,
            Boolean = 6,
            Enum = 7,
            IntegerRange = 8,
            FloatColor = 9,
            FloatRange = 10,
        }

        [RTTI.Serializable(0xBDA1576BDD21EF20, 0x4)]
        public enum EPS4ProRenderMode : int32
        {
            PS4ProRenderModeHighResolution = 0,
            PS4ProRenderModeHighFramerate = 1,
        }

        [RTTI.Serializable(0x77CA907ECD86DA62, 0x4)]
        public enum EPanelScrollType : int32
        {
            Unset = -1,
            Horizontal = 0,
            Vertical = 1,
        }

        [RTTI.Serializable(0x97FC6B7B5F645216, 0x4)]
        public enum EParentObjectiveVisibilityLogic : int32
        {
            AlwaysShow = 0,
            ShowWhenSubObjectivesVisible = 1,
            ShowWhenSubObjectivesHidden = 2,
        }

        [RTTI.Serializable(0x1704CA8EACDB8CD3, 0x1)]
        public enum EParkourTransitionLimitAxis : int8
        {
            X = 0,
            Y = 1,
            Z = 2,
        }

        [RTTI.Serializable(0xF2B5133B41F41A92, 0x1)]
        public enum EParkourTransitionLimitSimpleShape : int8
        {
            Ellipse = 0,
            Box = 1,
        }

        [RTTI.Serializable(0xE9A326FB734453FF, 0x4)]
        public enum EParkourTransitionType : int32
        {
            Jump = 0,
            Corner = 1,
            PullUpToPerched = 2,
            LowerFromPerched = 3,
            AnnotationTraversal = 4,
            Turn = 5,
            ReleaseHang = 6,
        }

        [RTTI.Serializable(0xB46B16BF6FE86B1D, 0x4)]
        public enum EParticleCollisionMode : int32
        {
            RaycastCollision = 0,
            ScreenSpaceCollision = 1,
        }

        [RTTI.Serializable(0x78FE3F51E7D41E1B, 0x4)]
        public enum EParticleControlledAttributeSource : int32
        {
            None = 0,
            Lifetime = 1,
            Lifespan = 2,
            Velocity = 3,
            Random = 4,
        }

        [RTTI.Serializable(0xE8A9011ECBA922D4, 0x4)]
        public enum EParticleEmitRateUnits : int32
        {
            ParticlesPerSecond = 0,
            ParticlesPerMeter = 1,
        }

        [RTTI.Serializable(0x69BC2CA5DF064AF7, 0x4)]
        public enum EParticleFadeMode : int32
        {
            No_Fading = 0,
            Per_Particle_Fading = 1,
        }

        [RTTI.Serializable(0xC84A754C090715AF, 0x4)]
        public enum EParticlePivotAligment : int32
        {
            Top = 0,
            Center = 1,
            Bottom = 2,
        }

        [RTTI.Serializable(0x6761C66AA4BA669F, 0x4)]
        public enum EParticleShape : int32
        {
            FlatQuad = 0,
            TentedQuad = 1,
            PolyTrail = 2,
            Octagonal = 3,
        }

        [RTTI.Serializable(0xFBA2808F9C8754F6, 0x4)]
        public enum EParticleSubTexAnimationSrc : int32
        {
            ParticleAge = 0,
            ParticleLifetime = 1,
            ParticleVelocity = 2,
        }

        [RTTI.Serializable(0x4DF4CCA30383B526, 0x4)]
        public enum EParticleSystemUpdateMode : int32
        {
            Always = 0,
            WhenVisible = 1,
        }

        [RTTI.Serializable(0xC98E6D7AB57F3F6D, 0x4)]
        public enum EPathMode : int32
        {
            Time = 0,
            Distance = 1,
        }

        [RTTI.Serializable(0x6CFF437E96A6E0D3, 0x1)]
        public enum EPerkAbility : int8
        {
            HorseCall = 0,
            LureEnemy = 1,
            Invalid = -1,
        }

        [RTTI.Serializable(0x3F5FE3137F79609F, 0x4)]
        public enum EPerkPointGainReason : int32
        {
            Initial = 0,
            Restore = 1,
            LevelUp = 2,
            Quest = 3,
            Script = 4,
            Debug = 5,
        }

        [RTTI.Serializable(0x42F35B8C3AC3E20F, 0x4)]
        public enum EPhysicsCollisionLayerGame : int32
        {
            Static = 1,
            Dynamic_HQ = 2,
            Dynamic = 3,
            Water_raycast = 4,
            Sound_occlusion = 5,
            Ragdoll = 6,
            Water = 7,
            Gravity_pockets = 50,
            Static_shoot_through = 51,
            Dynamic_shoot_through = 52,
            Bullet_blocker = 53,
            Bullet_blocker_raycast = 54,
            Trigger = 55,
            Trigger_raycast = 56,
            Heavy_Ragdoll = 57,
            Player = 8,
            AI_or_Remote_Player = 9,
            Humanoid_blocker = 10,
            Player_blocker = 11,
            Foot_placement = 12,
            Dynamic_but_humanoid = 13,
            Humanoid_raycast_movement = 14,
            VIP_ragdoll = 15,
            Ragdoll_no_collision_vs_static = 16,
            Vehicle = 17,
            Vehicle_stopper = 18,
            Humanoid_movement_helper = 19,
            Projectile = 20,
            Character_high_quality = 21,
            Vehicle_no_static = 22,
            AI_static = 23,
            Dive_Query = 24,
            vs_Humanoids = 25,
            VIP_stopper = 26,
            VIP_AI = 27,
            Ragdoll_stopper = 28,
            Mortally_wounded = 29,
            Dynamic_HQ_but_humanoid = 30,
            Proxy_player = 31,
            Blocks_AI_Hearing = 32,
            Vehicle_only = 33,
            Dynamic_no_vehicles = 34,
            Amphibious_Foot_placement = 35,
            Blocks_vision = 36,
            Player_Ragdoll = 37,
            Proxy_player_2 = 38,
            Blocks_AI_Hearing_Raycast = 39,
            Weapon_blocker = 40,
            Walkthrough_shield = 41,
            Zipline_raycast = 42,
            Zipline_blocker = 43,
            Static_But_Humanoid = 44,
            Weapon_Blocker_Static = 45,
            Particles_Collision = 46,
            Ray_vs_Static = 47,
            Entity_Placement_Ability = 48,
            Camera_Obstruction = 58,
            Navigation_Mesh = 59,
            Vault_Query = 60,
            Vs_Static_and_Bullet_Blocker = 61,
            Deep_Water_Surface = 62,
            Amphibious_Navigation_Mesh = 63,
            Navigation_Mesh_Hard_Obstacle = 64,
            Blocking_Shield = 65,
            Camera_Collision = 66,
            Static_but_Navigation_Mesh = 67,
            Lightbake_Visibility = 68,
            Foot_Support = 69,
            Dynamic_HQ_but_FOOT_Support = 70,
            Humanoid_raycast_movement_no_ragdoll = 71,
            Camera_Blocker = 72,
            Player_and_Camera_Blocker = 73,
            Ray_vs_Static___Water = 74,
            Air_Movement_Blocker = 75,
            Dynamic_HQ_but_human_and_Air_Movement = 76,
            Dynamic_But_Ragdolls = 77,
            Static_but_Blocks_Vision = 78,
            Camera_Blocker_Raycast = 79,
            Air_Navigation = 80,
            Navigation_Mesh_Only = 81,
            Static_Debug = 82,
            Dynamic_Debug = 83,
            Density_Debug = 84,
        }

        [RTTI.Serializable(0xCA6771B36653C1A7, 0x1)]
        public enum EPhysicsInitMode : int8
        {
            Default = 0,
            Active = 1,
            Inactive = 2,
        }

        [RTTI.Serializable(0xDCF243A8FB8CD72E, 0x4)]
        public enum EPhysicsMotionType : int32
        {
            Dynamic = 1,
            Keyframed = 2,
            Static = 3,
        }

        [RTTI.Serializable(0xE45109ECA9FFAE9A, 0x4)]
        public enum EPhysicsQualityType : int32
        {
            Default = 0,
            Debris = 1,
            Moving = 2,
            Keyframed = 3,
            Fixed = 4,
            Bullet = 5,
            Critical = 6,
            Grenade = 7,
            Projectile = 8,
        }

        [RTTI.Serializable(0x5BA2FA06D0CD9A28, 0x4)]
        public enum EPhysicsShapeType : int32
        {
            InvalidShape = 0,
            Auto = 1,
            Sphere = 2,
            Cylinder = 3,
            Box = 4,
            ConvexHull = 5,
            PolySoup = 7,
            ConvexShape = 6,
            Compound = 8,
            Capsule = 9,
            HeightMap = 10,
            CompressedPolySoup = 11,
        }

        [RTTI.Serializable(0xA60DEC15882A341F, 0x1)]
        public enum EPickUpAnimationWieldDirective : int8
        {
            DoNothing = 0,
            StowWeapon = 1,
            SwitchToMeleeWeaponImmidiately = 2,
        }

        [RTTI.Serializable(0x2CB3675607AF1068, 0x4)]
        public enum EPixelFormat : int32
        {
            INVALID = 76,
            RGBA_5551 = 0,
            RGBA_5551_REV = 1,
            RGBA_4444 = 2,
            RGBA_4444_REV = 3,
            RGB_888_32 = 4,
            RGB_888_32_REV = 5,
            RGB_888 = 6,
            RGB_888_REV = 7,
            RGB_565 = 8,
            RGB_565_REV = 9,
            RGB_555 = 10,
            RGB_555_REV = 11,
            RGBA_8888 = 12,
            RGBA_8888_REV = 13,
            RGBE_REV = 14,
            RGBA_FLOAT_32 = 15,
            RGB_FLOAT_32 = 16,
            RG_FLOAT_32 = 17,
            R_FLOAT_32 = 18,
            RGBA_FLOAT_16 = 19,
            RGB_FLOAT_16 = 20,
            RG_FLOAT_16 = 21,
            R_FLOAT_16 = 22,
            RGBA_UNORM_32 = 23,
            RG_UNORM_32 = 24,
            R_UNORM_32 = 25,
            RGBA_UNORM_16 = 26,
            RG_UNORM_16 = 27,
            R_UNORM_16 = 28,
            RGBA_UNORM_8 = 29,
            RG_UNORM_8 = 30,
            R_UNORM_8 = 31,
            RGBA_NORM_32 = 32,
            RG_NORM_32 = 33,
            R_NORM_32 = 34,
            RGBA_NORM_16 = 35,
            RG_NORM_16 = 36,
            R_NORM_16 = 37,
            RGBA_NORM_8 = 38,
            RG_NORM_8 = 39,
            R_NORM_8 = 40,
            RGBA_UINT_32 = 41,
            RG_UINT_32 = 42,
            R_UINT_32 = 43,
            RGBA_UINT_16 = 44,
            RG_UINT_16 = 45,
            R_UINT_16 = 46,
            RGBA_UINT_8 = 47,
            RG_UINT_8 = 48,
            R_UINT_8 = 49,
            RGBA_INT_32 = 50,
            RG_INT_32 = 51,
            R_INT_32 = 52,
            RGBA_INT_16 = 53,
            RG_INT_16 = 54,
            R_INT_16 = 55,
            RGBA_INT_8 = 56,
            RG_INT_8 = 57,
            R_INT_8 = 58,
            RGB_FLOAT_11_11_10 = 59,
            RGBA_UNORM_10_10_10_2 = 60,
            RGB_UNORM_11_11_10 = 61,
            DEPTH_FLOAT_32_STENCIL_8 = 62,
            DEPTH_FLOAT_32_STENCIL_0 = 63,
            DEPTH_24_STENCIL_8 = 64,
            DEPTH_16_STENCIL_0 = 65,
            BC1 = 66,
            BC2 = 67,
            BC3 = 68,
            BC4U = 69,
            BC4S = 70,
            BC5U = 71,
            BC5S = 72,
            BC6U = 73,
            BC6S = 74,
            BC7 = 75,
        }

        [RTTI.Serializable(0x2407E8007C424BC7, 0x4)]
        public enum EPlacementChunkSizeSetting : int32
        {
            Auto = 0,
            Small = 1,
            Medium = 2,
            Large = 3,
        }

        [RTTI.Serializable(0x96167B3CAD850063, 0x4)]
        public enum EPlacementPrecision : int32
        {
            Normal = 0,
            Conservative = 1,
        }

        [RTTI.Serializable(0xC2979D608C1E2B5A, 0x4)]
        public enum EPlacementUsageMask : int32
        {
            ObserverOnly = 1,
            AreaOnly = 2,
            All = 3,
        }

        [RTTI.Serializable(0xD8FED11BDA200CDC, 0x4)]
        public enum EPlayerCardUnlockTier : int32
        {
            NoTier = -1,
            Tier1 = 0,
            Tier2 = 1,
            Tier3 = 2,
            Tier4 = 3,
            TierCount = 4,
        }

        [RTTI.Serializable(0xEF7AE43096A9D383, 0x1)]
        public enum EPlayerChoiceLocation : int8
        {
            Auto = -1,
            TopRight = 0,
            MiddleRight = 1,
            BottomRight = 2,
            BottomLeft = 3,
            MiddleLeft = 4,
            TopLeft = 5,
        }

        [RTTI.Serializable(0x61150FD8147D2B8C, 0x4)]
        public enum EPlayerHealthSettings : int32
        {
            Low = 0,
            Normal = 1,
            High = 2,
        }

        [RTTI.Serializable(0x7DBD36E7AD4263B3, 0x4)]
        public enum EPlayerKilledDataBits : int32
        {
            None = -1,
            Headshot = 1,
            Zoomed = 2,
            CloseCombat = 3,
            Crouched = 4,
            Cover = 5,
            ThroughShield = 6,
            Capture_Attacker = 7,
            Capture_Defender = 8,
            CnH_Attacker = 9,
            CnH_Defender = 10,
            CnS_Attacker = 11,
            CnS_Defender = 12,
            CnC_Attacker = 13,
            CnC_Defender = 14,
            Search_Attacker = 15,
            Search_Defender = 16,
            VictimIsFlagCarrier = 17,
            SnR_Attacker = 18,
            SnR_Defender = 19,
            SnR_VictimFlagCarrier = 20,
            InR_Attacker = 21,
            InR_Defender = 22,
            InR_VictimFlagCarrier = 23,
            SnS_VictimFlagCarrier = 24,
            SnD_Attacker = 25,
            SnD_Defender = 26,
            KillerCloakActive = 27,
            KillerArmadilloActive = 28,
            KillerSpeedDashActive = 29,
            VictimCloakActive = 31,
            VictimArmadilloActive = 32,
            VictimSpeedDashActive = 33,
            VictimKillingSpree = 36,
            VictimZipLine = 37,
            VictimIsArmingSnDObject = 38,
            VictimIsDisarmingSnDObject = 39,
        }

        [RTTI.Serializable(0xE22565490DB643BB, 0x4)]
        public enum EPlayerNumber : int32
        {
            None = -1,
            _0 = 0,
            _1 = 1,
            _2 = 2,
            _3 = 3,
        }

        [RTTI.Serializable(0xBF09ECF8E4B927F9, 0x1)]
        public enum EPlayerResourceInventorySortOrder : int8
        {
            RarityAsc = 0,
            RarityDesc = 1,
            ItemSellValueAsc = 2,
            ItemSellValueDesc = 3,
            ItemCategory = 4,
        }

        [RTTI.Serializable(0xCD0194863FCB911D, 0x4)]
        public enum EPlayerScoreEntityStat : int32
        {
            None = -1,
            k = 0,
            wf = 1,
            ht = 2,
            hs = 3,
            st = 6,
            tu = 4,
            ds = 5,
        }

        [RTTI.Serializable(0x3AE78EDFD8255261, 0x4)]
        public enum EPlayerScoreStat : int32
        {
            None = -1,
            Score = 0,
            GamePoints = 70,
            GamesPlayed = 1,
            GamesKicked = 2,
            TimePlayed = 66,
            GameWin = 3,
            GameLosses = 4,
            GameDraws = 5,
            Kills = 6,
            KillsAssists = 7,
            MeleeKills = 8,
            EntityKills = 9,
            Deaths = 10,
            Suicides = 11,
            TeamKills = 12,
            CurKillSpree = 13,
            HighKillSpree = 14,
            TotalHeadShots = 15,
            CAH_captured = 16,
            CAH_neutralized = 17,
            CAH_defender_kill = 18,
            CAH_attacker_kill = 19,
            CAS_captured = 20,
            CAS_neutralized = 21,
            CAS_defender_kill = 22,
            CAS_attacker_kill = 23,
            CAC_captured = 24,
            CAC_neutralized = 25,
            CAC_defender_kill = 26,
            CAC_attacker_kill = 27,
            SAR_picked_up = 28,
            SAR_retrieved = 29,
            SAR_defender_kill = 30,
            SAR_attacker_kill = 31,
            SAR_killed_carrier = 32,
            IAR_picked_up = 33,
            IAR_retrieved = 34,
            IAR_defender_kill = 35,
            IAR_attacker_kill = 36,
            IAR_killed_carrier = 37,
            SAS_picked_up = 38,
            SAS_speaker_carried = 39,
            SAS_killed_carrier = 40,
            SAD_detonated = 41,
            SAD_demolition_man = 42,
            SAD_demolition_expert = 43,
            SAD_armed = 44,
            SAD_disarmed = 45,
            SAD_defender_kill = 46,
            SAD_attacker_kill = 47,
            BodycountKills = 48,
            AbSentryTurret = 49,
            AbDisguise = 50,
            AbHeal = 51,
            AbHack = 52,
            AbRepair = 53,
            AbRevive = 54,
            AbCloakedKills = 55,
            AbArmadilloKills = 56,
            AbSpawnBeaconUsed = 57,
            AbSupplyBoxUsed = 58,
            AbTacticalEchoEnemyTags = 59,
            MortarStrikes = 60,
            ZipLineKills = 67,
            HeadshotsInCurrentFocusMode = 68,
            CivilianKillsInCurrentNode = 69,
            OverallRank = 61,
            MissionLosses = 62,
            MissionWins = 63,
            MissionPoints = 64,
            LevelEvent = 65,
            LagCount = 71,
        }

        [RTTI.Serializable(0xDA678F9AB9CBEE69, 0x4)]
        public enum EPlaylistFilterOperation : int32
        {
            EQUALS = 0,
            NOT_EQUALS = 1,
            CONTAINS_ALL = 2,
            NOT_CONTAINS_ALL = 3,
            CONTAINS_ANY = 4,
            CONTAINS_NONE = 5,
        }

        [RTTI.Serializable(0x85175B948B069BBA, 0x4)]
        public enum EPointOfAimRotation : int32
        {
            Camera = 0,
            Chest = 1,
            Position = 2,
        }

        [RTTI.Serializable(0x545E218E08406084, 0x4)]
        public enum EPositionAimMode : int32
        {
            LowestTrajectory = 0,
            HighestTrajectory = 1,
        }

        [RTTI.Serializable(0xA55A2A1531595EDB, 0x4)]
        public enum EPositionAssessment : int32
        {
            invalid = -1,
            observed_exact = 0,
            deduced_exact = 1,
            deduced_rough = 2,
            deduced_unknown = 3,
            confirmed_lost = 4,
        }

        [RTTI.Serializable(0x3B32A1820C6E08C7, 0x4)]
        public enum EPostProcessBlendMode : int32
        {
            Lerp = 0,
            Add = 1,
        }

        [RTTI.Serializable(0x3A5CEF69D6CEFCC7, 0x4)]
        public enum EPreviewProjectileCreateMode : int32
        {
            Wielding = 0,
            Charging = 1,
        }

        [RTTI.Serializable(0x843528EE6059C3D2, 0x4)]
        public enum EPriceModificationType : int32
        {
            Buy = 0,
            Sell = 1,
        }

        [RTTI.Serializable(0x536FF2B448C227A8, 0x4)]
        public enum EPrimitiveSkinInfoType : int32
        {
            Basic = 0,
            NBT = 1,
            VsBasic = 2,
            VsNbt = 3,
            CsNrm = 4,
            CsNbt = 5,
            CsNrmGen = 6,
            CsNbtGen = 7,
        }

        [RTTI.Serializable(0xD6491AA100C34AB7, 0x4)]
        public enum EProcessReturnValue : int32
        {
            Crashed = -1,
            Success = 0,
            Failure = 1,
            Mission_failed = 2,
            Time_out = 3,
            Alerts = 4,
            Memory_error = 9,
            NetworkError = 5,
            Network_server_timeout = 7,
        }

        [RTTI.Serializable(0xB9098BBD2DA1DF39, 0x4)]
        public enum EProfileUpdateStatus : int32
        {
            SUCCESS = 0,
            ALREADY_APPLIED = 1,
            TRY_AGAIN = 2,
            INVALID_UPDATE = 3,
        }

        [RTTI.Serializable(0x8E32C40558E36E3E, 0x4)]
        public enum EProgramType : int32
        {
            VertexProgram = 2,
            GeometryProgram = 1,
            PixelProgram = 3,
            ComputeProgram = 0,
        }

        [RTTI.Serializable(0x2D12E73211551897, 0x4)]
        public enum EProgramTypeMask : int32
        {
            VP = 4,
            GP = 2,
            FP = 8,
            CP = 1,
            All = 15,
            AllGraphics = 14,
            VP_GP_FP = 14,
            VP_GP = 6,
            VP_FP = 12,
            FP_CP = 9,
            None = 0,
        }

        [RTTI.Serializable(0x970A945CF8BC4B59, 0x4)]
        public enum EProjColorOperation : int32
        {
            None = 0,
            Replace = 1,
            Multiply = 2,
            Add = 3,
            Subtract = 4,
        }

        [RTTI.Serializable(0x43620D9A4C9F2E30, 0x4)]
        public enum EProjectionMode : int32
        {
            Perspective = 0,
            Orthogonal = 1,
        }

        [RTTI.Serializable(0xD0A79FE132BE65DA, 0x4)]
        public enum EQuestObjectiveType : int32
        {
            Default = 0,
            Optional = 1,
        }

        [RTTI.Serializable(0xE79A08F9FE511B35, 0x1)]
        public enum EQuestRunState : int8
        {
            Running = 0,
            Paused = 1,
            UniqueBlocked = 2,
            Cooldown = 4,
        }

        [RTTI.Serializable(0x8834880540C5024, 0x4)]
        public enum EQuestSectionCompletionType : int32
        {
            Any = 0,
            All = 1,
        }

        [RTTI.Serializable(0x2C2AB5EB7B811D0, 0x4)]
        public enum EQuestSectionState : int32
        {
            Unavailable = 0,
            Available = 1,
            Completed = 2,
            Blocked = 3,
        }

        [RTTI.Serializable(0xA9DC92B16858930A, 0x4)]
        public enum EQuestSectionType : int32
        {
            Start = 0,
            Progress = 1,
            Success = 2,
            Fail = 3,
        }

        [RTTI.Serializable(0xF108F2A42224D0DD, 0x4)]
        public enum EQuestState : int32
        {
            Unavailable = 0,
            Available = 1,
            InProgress = 2,
            Succeeded = 3,
            Failed = 4,
        }

        [RTTI.Serializable(0x3298C3B739F5F4D, 0x1)]
        public enum ERandomShaderVariableType : int8
        {
            DontRandomize = 0,
            SingleRandomValueForAllParts = 1,
            RandomValuePerPart = 2,
        }

        [RTTI.Serializable(0xE38BC840DD2431F8, 0x4)]
        public enum EReactionEndType : int32
        {
            Finish = 0,
            Skip = 1,
            Decay = 2,
            Abort = 3,
        }

        [RTTI.Serializable(0x33996D681A67A36F, 0x4)]
        public enum EReactionPassThroughType : int32
        {
            Stop_Here = 0,
            Skip_and_Continue = 1,
            Play_and_Continue = 2,
        }

        [RTTI.Serializable(0x9C7D8847E3850992, 0x4)]
        public enum ERegion : int32
        {
            Invalid = -1,
            Europe1 = 0,
            Europe2 = 1,
            Europe3 = 2,
            US = 3,
            LatinAmerica = 4,
            Japan = 5,
            Asia = 6,
            China = 7,
            UK = 8,
            Germany = 9,
            US_Combined = 10,
            Global = 11,
            Test = 12,
        }

        [RTTI.Serializable(0xA423FFDDD40AECD5, 0x4)]
        public enum ERelativeUseLocationPosition : int32
        {
            ORIGIN = 0,
            ANIMATION_START = 1,
            ANIMATION_FOLLOW = 2,
        }

        [RTTI.Serializable(0xFA671FD95C527BA, 0x1)]
        public enum ERenderDataHintDataType : int8
        {
            FrameBased = 0,
            GridBased = 1,
            AreaBased = 2,
            Invalid = 3,
        }

        [RTTI.Serializable(0x624C2EAC411306B9, 0x4)]
        public enum ERenderEffectType : int32
        {
            Object_render_effect = 0,
            Spotlight_render_effect = 1,
            Omnilight_render_effect = 2,
            Sunlight_render_effect = 3,
        }

        [RTTI.Serializable(0xE2EF6D06E5120F58, 0x1)]
        public enum ERenderPlatform : int8
        {
            DX = 0,
            DX12 = 1,
            PINK = 2,
            PS5 = 3,
            Invalid = 4,
        }

        [RTTI.Serializable(0x464DA288B9E39D4F, 0x4)]
        public enum ERenderTechniqueSetType : int32
        {
            Invalid_rendering_techniques = -1,
            Normal_rendering_techniques = 0,
            Instanced_techniques = 1,
        }

        [RTTI.Serializable(0x9287F7681743E6AB, 0x4)]
        public enum ERenderTechniqueType : int32
        {
            Invalid = -1,
            Direct = 0,
            Unlit = 1,
            DepthOnly = 2,
            Deferred = 3,
            DeferredEmissive = 4,
            DeferredTransAcc = 5,
            DeferredTrans = 6,
            CustomDeferred = 7,
            HalfDepthOnly = 8,
            LightSampling = 9,
            CustomForward = 10,
            Transparency = 11,
            ForwardBackground = 12,
            ForwardHalfRes = 13,
            ForwardQuarterRes = 14,
            ForwardMotionVectors = 15,
            ForwardForeground = 16,
            VolumeLightAmount = 17,
            Shadowmap = 18,
        }

        [RTTI.Serializable(0x99538AF4793441A8, 0x4)]
        public enum ERenderZoneFadeRegion : int32
        {
            Inwards = 0,
            Outwards = 1,
        }

        [RTTI.Serializable(0xE060ECC7704CD7DC, 0x1)]
        public enum ERequiredJumpMovementState : int8
        {
            Unrestricted = 0,
            Moving = 1,
            StandingStill = 2,
        }

        [RTTI.Serializable(0xE1CB960EE4AA8B0A, 0x1)]
        public enum ERewardStackingDirection : int8
        {
            Horizontal = 0,
            Vertical = 1,
        }

        [RTTI.Serializable(0x260D751B3FCB588B, 0x4)]
        public enum ERoadBakeDataMode : int32
        {
            None = 0,
            Height = 1,
            Topo_Roads = 2,
            HeightAndTopoRoads = 3,
        }

        [RTTI.Serializable(0xBE77D61B072EEDC1, 0x4)]
        public enum ERoadNodeProfileType : int32
        {
            Path = 0,
            Trail = 1,
            Trail_Snow = 2,
            Road = 3,
        }

        [RTTI.Serializable(0xFFB895E2CC939A6B, 0x4)]
        public enum ERoadNodeSnapMode : int32
        {
            Snap_To_Terrain_Height = 0,
            Use_Road_Height = 1,
        }

        [RTTI.Serializable(0xC8F130894EA1BF7, 0x4)]
        public enum ERootBoneMode : int32
        {
            Relative = 0,
            Absolute = 1,
            None = 2,
        }

        [RTTI.Serializable(0x7078D47645581813, 0x4)]
        public enum ERopeMode : int32
        {
            Anchor = 0,
            Tripwire = 1,
            RopePath = 2,
        }

        [RTTI.Serializable(0x66C132CFBABB0B31, 0x4)]
        public enum ERotationType : int32
        {
            RotationNone = 0,
            RotationZ = 1,
            RotationFull = 2,
        }

        [RTTI.Serializable(0x11B45BBA6D3DDF0, 0x1)]
        public enum ESRTCreationMode : int8
        {
            SplitPerProgramType = 0,
            Merged = 1,
            Inline = 2,
            Invalid = 3,
        }

        [RTTI.Serializable(0x9F49254D0FC4DED0, 0x1)]
        public enum ESRTElementFormat : int8
        {
            Unknown = 0,
            half = 1,
            half2 = 2,
            half3 = 3,
            half4 = 4,
            _float = 5,
            float2 = 6,
            float3 = 7,
            float4 = 8,
            _int = 9,
            int2 = 10,
            int3 = 11,
            int4 = 12,
            _uint = 13,
            uint2 = 14,
            uint3 = 15,
            uint4 = 16,
            float2x3 = 17,
            float3x4 = 18,
            float4x4 = 19,
            subset = 20,
        }

        [RTTI.Serializable(0x39D7BC69A14E495E, 0x1)]
        public enum ESRTElementType : int8
        {
            Unknown = 0,
            Constant = 1,
            Texture1D = 2,
            Texture2D = 3,
            Texture3D = 4,
            TextureCube = 5,
            Texture2DArray = 6,
            TextureIrradianceVolume = 7,
            RWTexture2D = 8,
            RWTexture2DArray = 9,
            RWTexture3D = 10,
            Sampler = 11,
            ShadowSampler = 12,
            DataBuffer = 13,
            StructuredBuffer = 14,
            RWDataBuffer = 15,
            RWStructuredBuffer = 16,
        }

        [RTTI.Serializable(0xA08CF04A2320CB13, 0x1)]
        public enum ESRTStorageMode : int8
        {
            ShaderInstance = 0,
            Scratch = 1,
        }

        [RTTI.Serializable(0xCECF30CE03BF586B, 0x1)]
        public enum ESRTUpdateFrequency : int8
        {
            Static = 0,
            PerFrame = 1,
            PerPass = 2,
            PerView = 3,
            PerTile = 4,
            PerBatch = 5,
            PerInstance = 6,
            Count = 7,
        }

        [RTTI.Serializable(0x1ED862BA3F778BF7, 0x1)]
        public enum ESaveGameSlot : int8
        {
            Memory = -2,
            Auto = -1,
            Slot0 = 0,
            Slot1 = 1,
            Slot2 = 2,
            Slot3 = 3,
            Slot4 = 4,
        }

        [RTTI.Serializable(0x293ABDE694DE61FE, 0x1)]
        public enum ESaveGameType : int8
        {
            Manual = 1,
            Quick = 2,
            Automatic = 4,
            All = 15,
        }

        [RTTI.Serializable(0xFABE540EDE656A7F, 0x4)]
        public enum EScaleBehaviour : int32
        {
            NoScaling = 0,
            ScaleWhenIncreased = 1,
            ScaleWhenDecreased = 2,
            ScaleBothWays = 3,
        }

        [RTTI.Serializable(0xD8CDB8376AF31728, 0x1)]
        public enum ESceneActivationTriggerType : int8
        {
            ActivationTrigger = 0,
            DeactivationTrigger = 1,
        }

        [RTTI.Serializable(0xF17B0B3029B5B102, 0x4)]
        public enum ESceneActivationType : int32
        {
            Normal = 0,
            Large = 1,
            OwnedByParent = 2,
            Global = 3,
        }

        [RTTI.Serializable(0xFA44A5F205BC780E, 0x1)]
        public enum ESceneForcedActiveState : int8
        {
            NoForcedState = 0,
            ForcedActive = 1,
            ForcedInactive = 2,
        }

        [RTTI.Serializable(0xF3EFAB9E0AAE0DFE, 0x4)]
        public enum EScoreEvent : int32
        {
            None = -1,
            Kill = 0,
            KillHeadshot = 1,
            KillExplosive = 2,
            KillAssist = 3,
            KillAssistMelee = 4,
            KillMelee = 5,
            KillMercy = 6,
            TeamKill = 7,
            Suicide = 8,
            SuicideBaseGun = 9,
            EntityKill = 10,
            FirstBlood = 11,
            KnockOut = 12,
            MissionWin = 13,
            KillCover = 14,
            KillThroughShield = 15,
            KillWhileUsingArmadillo = 16,
            KillArmadillo = 17,
            KillWhileUsingCloak = 18,
            KillCloak = 19,
            KillSavior = 20,
            KillRevenge = 21,
            KillPayback = 22,
            KillZipLine = 23,
            KillStun = 24,
            KillStunAssist = 25,
            SpawnAssist = 26,
            Supplier = 27,
            EnemyDeconstruction = 28,
            DoubleKill = 29,
            TripleKill = 30,
            MultiKill = 31,
            TwinKill = 32,
            ManyKill = 33,
            KillStreakThree = 34,
            KillStreakFive = 35,
            KillStreakTen = 36,
            KillStreakFifteen = 37,
            KillStreakTwenty = 38,
            KillStreakStopper = 39,
            Hack = 40,
            Repair = 41,
            Revive = 42,
            Mission = 43,
            LivesRemaining = 44,
            LastPlayerRemaining = 45,
            EnemyStunned = 46,
            Armed = 47,
            Disarmed = 48,
            SADAttackerKill = 49,
            SADDefenderKill = 50,
            DemolitionMan = 51,
            DemolitionExpert = 52,
            Captured = 53,
            Neutralized = 54,
            AttackerKill = 55,
            DefenderKill = 56,
            SarFlagPickup = 57,
            SarFlagRetrieved = 58,
            SarCarrierKilled = 59,
            SarCarrierSavior = 60,
            SarFlagCarrying = 61,
            SarAttackerKill = 62,
            SarDefenderKill = 63,
            IarFlagPickup = 64,
            IarFlagRetrieved = 65,
            IarFlagReturned = 66,
            IarCarrierKilled = 67,
            IarCarrierSavior = 68,
            IarFlagCarrying = 69,
            IarAttackerKill = 70,
            IarDefenderKill = 71,
            SasFlagPickup = 72,
            SasCarrierKilled = 73,
            SasCarrierSavior = 74,
            SasFlagCarrying = 75,
            AssKill = 76,
            AssWound = 77,
            AssMercy = 78,
            Switch = 79,
            Task = 80,
        }

        [RTTI.Serializable(0x1292E222E74AC11A, 0x1)]
        public enum ESelectByFactContext : int8
        {
            Default = 0,
            Global = 1,
            Player = 2,
            Parent = 3,
        }

        [RTTI.Serializable(0x61E51DC75945F0B5, 0x1)]
        public enum ESelectByPropertyContext : int8
        {
            Default = 0,
            Player = 1,
            Parent = 2,
        }

        [RTTI.Serializable(0x2D2754F564ECE415, 0x4)]
        public enum ESelfDamage : int32
        {
            Yes = 1,
            No = 2,
            All = 3,
        }

        [RTTI.Serializable(0x22D6EBE2514387AF, 0x4)]
        public enum ESelfShadowMode : int32
        {
            None = 0,
            Fake = 1,
            Occlusion = 2,
        }

        [RTTI.Serializable(0x8115E0D0FAB1E332, 0x1)]
        public enum ESentenceDelivery : int8
        {
            on_actor = 1,
            radio = 2,
            proximity = 3,
        }

        [RTTI.Serializable(0x83F51A3458088ABB, 0x4)]
        public enum ESentenceGroupType : int32
        {
            Normal = 0,
            OneOfRandom = 1,
            OneOfInOrder = 2,
        }

        [RTTI.Serializable(0xA75A3DB199D3018B, 0x1)]
        public enum ESequenceFactContextType : int8
        {
            Global = 0,
            Scene = 1,
            Actor = 2,
        }

        [RTTI.Serializable(0xE24318DA890841A8, 0x4)]
        public enum ESequenceHideBehavior : int32
        {
            Hide = 0,
            Remove = 1,
        }

        [RTTI.Serializable(0x9EE096ACC615438F, 0x1)]
        public enum ESequenceLoopMode : int8
        {
            Off = 0,
            Looping = 1,
        }

        [RTTI.Serializable(0xA473FDAAB2501982, 0x4)]
        public enum ESequenceNetworkBranchSelectionMode : int32
        {
            First = 0,
            Ordered = 1,
            Random = 2,
            Random_Unique = 3,
        }

        [RTTI.Serializable(0x4D9F50F5707A8A79, 0x4)]
        public enum ESequenceNetworkFactContextType : int32
        {
            Global = 0,
            Local = 1,
            Scene = 2,
        }

        [RTTI.Serializable(0x6AE3CAE3A25932A2, 0x1)]
        public enum ESequenceNetworkTransitionSourceType : int8
        {
            None = 0,
            Any = 1,
            DefaultNext = 2,
            DefaultInterrupt = 3,
            InterruptHandler = 4,
            PlayerChoice = 5,
        }

        [RTTI.Serializable(0x936E44BF139D4128, 0x1)]
        public enum ESequenceNetworkTransitionTargetType : int8
        {
            None = 0,
            Any = 1,
            SequenceNode = 2,
        }

        [RTTI.Serializable(0x9D930C9BC78F2FB, 0x1)]
        public enum EServerType : int8
        {
            Static = 0,
            Dynamic = 1,
        }

        [RTTI.Serializable(0xE09EAFED654EB665, 0x4)]
        public enum ESetFunctionHandled : int32
        {
            Never = 0,
            On_Trigger = 1,
            Always = 2,
        }

        [RTTI.Serializable(0x90D1135E676F8CB5, 0x1)]
        public enum EShaderInstancingMode : int8
        {
            None = 0,
            MaterialInstancing = 1,
            OnTheFly = 2,
            Invalid = 3,
        }

        [RTTI.Serializable(0x8FDCE6610E3C8B14, 0x1)]
        public enum EShaderVariableType : uint8
        {
            Float1 = 1,
            Float2 = 2,
            Float3 = 3,
            Float4 = 4,
            Uint1 = 9,
            Uint2 = 10,
            Uint3 = 11,
            Uint4 = 12,
            ShaderFloat1 = 33,
            ShaderFloat2 = 34,
            ShaderFloat3 = 35,
            ShaderFloat4 = 36,
            VertexFloat1 = 65,
            VertexFloat2 = 66,
            VertexFloat3 = 67,
            VertexFloat4 = 68,
            ConstFloat1 = 97,
            ConstFloat2 = 98,
            ConstFloat3 = 99,
            ConstFloat4 = 100,
            InstanceDataOffsetFloat1 = 129,
            InstanceDataOffsetFloat2 = 130,
            InstanceDataOffsetFloat3 = 131,
            InstanceDataOffsetFloat4 = 132,
        }

        [RTTI.Serializable(0x5A2AC6027CA00F1E, 0x4)]
        public enum EShadowBiasMode : int32
        {
            Multiplier = 0,
            AbsoluteBias = 1,
        }

        [RTTI.Serializable(0x4D351A964A2BE568, 0x4)]
        public enum EShadowCull : int32
        {
            Off = 0,
            CullFrontfaces = 2,
            CullBackfaces = 1,
        }

        [RTTI.Serializable(0x27118000B1CD9D7, 0x4)]
        public enum EShadowmapCacheForStaticGeometry : int32
        {
            No_cache_for_static_geometry = 0,
            Use_cache_for_static_geometry = 1,
            Use_cache_for_static_geometry__dynamic_geometry_ignored = 2,
            Map_size_varies_with_distance__cache_used_if___256 = 3,
            Map_size_varies_with_distance__cache_used_if___128 = 4,
        }

        [RTTI.Serializable(0xE3EDC2104D98A8A4, 0x1)]
        public enum EShowArcType : int8
        {
            Firing = 0,
            Aiming = 1,
            AimingNotFire = 2,
            WeaponIsActive = 3,
        }

        [RTTI.Serializable(0xC1FB87DB405534FE, 0x4)]
        public enum ESkinnedVtxType : int32
        {
            SKVTXTYPE_1x8 = 0,
            SKVTXTYPE_2x8 = 1,
            SKVTXTYPE_3x8 = 2,
            SKVTXTYPE_4x8 = 3,
            SKVTXTYPE_5x8 = 4,
            SKVTXTYPE_6x8 = 5,
            SKVTXTYPE_7x8 = 6,
            SKVTXTYPE_8x8 = 7,
            SKVTXTYPE_1x16 = 8,
            SKVTXTYPE_2x16 = 9,
            SKVTXTYPE_3x16 = 10,
            SKVTXTYPE_4x16 = 11,
            SKVTXTYPE_5x16 = 12,
            SKVTXTYPE_6x16 = 13,
            SKVTXTYPE_7x16 = 14,
            SKVTXTYPE_8x16 = 15,
        }

        [RTTI.Serializable(0x8DBFE39A12E9B721, 0x4)]
        public enum ESkinningDeformerType : int32
        {
            DeformPosAndNormals = 0,
            DeformPosAndComputeNormals = 1,
        }

        [RTTI.Serializable(0x44A9655D71FF03D7, 0x4)]
        public enum ESkipBehavior : int32
        {
            EndOfSequence = 0,
            EndOfEvent = 1,
            NotSkippable = 2,
        }

        [RTTI.Serializable(0x8265321AF4BE7261, 0x4)]
        public enum ESkipLocationType : int32
        {
            Invalid = -1,
            Start = 0,
            Intro = 1,
            Interlude = 2,
        }

        [RTTI.Serializable(0x44D26B29DA316E63, 0x4)]
        public enum ESortMode : int32
        {
            FrontToBack = 1,
            BackToFront = 2,
            Off = 0,
        }

        [RTTI.Serializable(0xEB72A8208EDBF65, 0x4)]
        public enum ESortOrder : int32
        {
            _0 = 0,
            _1 = 1,
            _2 = 2,
            _3 = 3,
            _4 = 4,
            _5 = 5,
            _6 = 6,
            _7 = 7,
            _8 = 8,
            _9 = 9,
            _10 = 10,
            _11 = 11,
            _12 = 12,
            _13 = 13,
            _14 = 14,
            _15 = 15,
        }

        [RTTI.Serializable(0xC27F1E066FB0BE16, 0x4)]
        public enum ESoundFilterMode : int32
        {
            Off = 0,
            Low_Pass = 1,
            High_Pass = 2,
            All_Pass = 3,
            Band_Pass = 4,
            Notch = 5,
            Peaking_EQ = 6,
            Low_Shelf = 7,
            High_Shelf = 8,
            Rendering = 9,
        }

        [RTTI.Serializable(0xF90F920DBE1A9465, 0x4)]
        public enum ESoundInstanceLimitMode : int32
        {
            Off = 0,
            Stop_Oldest = 1,
            Stop_Softest = 2,
            Reject_New = 3,
        }

        [RTTI.Serializable(0x6662E130032BA91C, 0x4)]
        public enum ESoundMasterVolumeGroup : int32
        {
            Player_Fire = 0,
            Robot_Fire = 1,
            Human_Fire = 2,
            Weapon_Reload_Charge = 3,
            Weapon_Impact_Large = 4,
            Weapon_Impact_Small = 5,
            Explosion = 6,
            Projectile_Whizzby = 7,
            Special_State = 8,
            MADDER = 9,
            GEERT = 10,
            Robot_Vocalization_Large = 11,
            Robot_Vocalization_Medium = 12,
            Robot_Vocalization_Small = 13,
            Physics = 14,
            Destructible_Small = 15,
            Destructible_Large = 16,
            Movement_Robot_Large = 17,
            Movement_Robot_Medium = 18,
            Movement_Robot_Small = 19,
            Movement_Human = 20,
            Movement_Player = 21,
            Wind = 22,
            Rain_Snow = 23,
            Thunder = 24,
            Fire = 25,
            Streaming_Water = 26,
            Bunker = 27,
            Machine = 28,
            Insect = 29,
            Reptile_Amphibian = 30,
            Bird = 31,
            Mammal = 32,
            Alarm = 33,
            Menu = 34,
            HUD = 35,
            Cutscene = 36,
            Music_Diegetic_3D_1 = 37,
            Music_Diegetic_3D_2 = 38,
            Pad_Speaker_SFX = 39,
            Dialogue_Player = 40,
            Dialogue_NPC_Scripted_Important = 41,
            Dialogue_NPC_Scripted = 42,
            Dialogue_NPC_AI_driven = 43,
            Walla = 44,
            Pad_Speaker_Dialogue = 45,
            Music_High_1 = 46,
            Music_High_2 = 47,
            Music_Low_1 = 48,
            Music_Low_2 = 49,
            UNASSIGNED = 51,
            USE_PARENT = 52,
        }

        [RTTI.Serializable(0xE3F1FC1D49839A79, 0x4)]
        public enum ESoundShape : int32
        {
            Sphere = 0,
            Box = 1,
            Cone = 2,
        }

        [RTTI.Serializable(0x829E57945677A69C, 0x4)]
        public enum ESoundZoneShapeType : int32
        {
            Sphere = 0,
            Box = 1,
            Cone = 2,
        }

        [RTTI.Serializable(0x71C65662CB115AF3, 0x1)]
        public enum ESpawnpointNavmeshPlacmentType : int8
        {
            NoPlacementOnNavmesh = 0,
            PointOnNavmesh = 1,
            FindNearestPointInRangeOnNavmesh = 2,
            FindRandomPointInRangeOnNavmesh = 3,
            FindNearestPointInRangeOnNavmeshWithRadialSpacing = 4,
            FindRandomPointInRangeOnNavmeshWithRadialSpacing = 5,
            FindNearestPointInRangeOnNavmeshOutOfSight = 6,
            FindRandomPointInRangeOnNavmeshOutOfSight = 7,
            PointInAirNav = 8,
            FindNearestPointInRangeInAirNav = 9,
            FindRandomPointInRangeInAirNav = 10,
            FindNearestPointInRangeInAirNavWithRadialSpacing = 11,
            FindRandomPointInRangeInAirNavWithRadialSpacing = 12,
            FindNearestPointInRangeInAirNavOutOfSight = 13,
            FindRandomPointInRangeInAirNavOutOfSight = 14,
            Default = 15,
        }

        [RTTI.Serializable(0x85FFED09F19E66C7, 0x4)]
        public enum EStacking : int32
        {
            _0 = 0,
            horizontal_tl = 1,
            horizontal_tr = 2,
            horizontal_bl = 3,
            horizontal_br = 4,
            vertical_tl = 5,
            vertical_tr = 6,
            vertical_bl = 7,
            vertical_br = 8,
        }

        [RTTI.Serializable(0x1FD41343B1A4E60C, 0x4)]
        public enum EStaminaType : int32
        {
            STAMINA = 0,
            ELECTRICITY = 1,
            BREATH = 2,
        }

        [RTTI.Serializable(0xE4BF315FA0FD26A8, 0x4)]
        public enum EStance : int32
        {
            INVALID = -1,
            STANDING = 0,
            CROUCHING = 1,
            LOWCROUCHING = 2,
        }

        [RTTI.Serializable(0x886C8BBC75BE0587, 0x4)]
        public enum EStatType : int32
        {
            Max = 0,
            Min = 1,
        }

        [RTTI.Serializable(0x9538E1D34D32C2A5, 0x1)]
        public enum EStencilBufferValue : int8
        {
            None = 0,
            Value1 = 1,
            Value2 = 2,
            Value3 = 3,
            Value4 = 4,
            Value5 = 5,
            Value6 = 6,
            Value7 = 7,
        }

        [RTTI.Serializable(0x1372E3AF4B27E78B, 0x4)]
        public enum EStick : int32
        {
            Invalid = -1,
            Left = 0,
            Right = 1,
        }

        [RTTI.Serializable(0xE65175DE8AFD5DA2, 0x4)]
        public enum EStickFunction : int32
        {
            Invalid = -1,
            Move = 0,
            Look = 1,
            InventorySelection = 2,
            DialogueChoice = 3,
            Zoom = 4,
            DialogueChoiceMouse = 6,
        }

        [RTTI.Serializable(0x6B8C7B2C6BC3E80E, 0x1)]
        public enum EStreamingLODLevel : int8
        {
            SuperLow = 0,
            Low = 1,
            Medium = 2,
            High = 3,
        }

        [RTTI.Serializable(0x45F600D02EB1567E, 0x4)]
        public enum ESubtitlePosition : int32
        {
            Bottom = 0,
            Top = 1,
        }

        [RTTI.Serializable(0xD8E4C44AE90C0CEF, 0x4)]
        public enum ESunCascadeShadowmapOverride : int32
        {
            StandardRenderShadowmap = 1,
            DontRenderShadowmapMakeFullyShadowed = 6,
            DontRenderShadowmapMakeFullyLit = 10,
        }

        [RTTI.Serializable(0xE3C8C033A3DF6E6C, 0x4)]
        public enum ESwayChange : int32
        {
            MaximalSway = 0,
            SmoothMaximalSway = 1,
            MinimalSway = 2,
            SmoothMinimalSway = 3,
            DontChangeSway = 4,
        }

        [RTTI.Serializable(0x50052A004027B793, 0x4)]
        public enum ETargetArrowType : int32
        {
            Default = 0,
            SearchAndRetrieve = 1,
            SearchAndRetrieveBase = 2,
            SearchAndSafeGuard = 3,
            SearchAndDestroyAttack = 4,
            CaptureAndHold = 6,
            CaptureAndSecure = 7,
            MortallyWounded = 8,
            MortallyWoundedMP = 9,
            SearchAndDestroyDefend = 5,
            Script = 12,
            DefendFriendly = 13,
            Medic = 10,
            CoopPlayer = 11,
            SpottedObjective = 14,
            SpottedObjectiveTracked = 15,
            SpottedEnemy = 16,
            SpottedEnvironmental = 17,
            DestroyHighPriority = 18,
        }

        [RTTI.Serializable(0x998A4334859D398C, 0x4)]
        public enum ETelemetryDamageTracking : int32
        {
            None = 0,
            By_Player = 1,
            By_AI = 2,
            All = 3,
        }

        [RTTI.Serializable(0xE566D72CCD1C5D3E, 0x4)]
        public enum ETerrainBorderStitchingMode : int32
        {
            Skirts = 0,
            IndexBuffer_Stitching = 1,
            None = 2,
        }

        [RTTI.Serializable(0x86030F1B9CFD66F7, 0x4)]
        public enum ETerrainHullShape : int32
        {
            TriMesh = 0,
            Extruded2d = 1,
        }

        [RTTI.Serializable(0x3509F64BCC9D93CF, 0x4)]
        public enum ETerrainMaterialLODType : int32
        {
            HighQuality = 0,
            Flattened = 1,
            LowLOD = 2,
        }

        [RTTI.Serializable(0xEE8C101D963FBAFD, 0x4)]
        public enum ETerrainTileCullingMode : int32
        {
            ViewCamera = 0,
            None = 1,
        }

        [RTTI.Serializable(0x57512BE59B68CFE7, 0x4)]
        public enum ETexAddress : int32
        {
            Wrap = 0,
            Clamp = 1,
            Mirror = 2,
            ClampToBorder = 3,
        }

        [RTTI.Serializable(0x4C5B52F1EADCA82F, 0x4)]
        public enum ETexColorSpace : int32
        {
            Linear = 0,
            sRGB = 1,
        }

        [RTTI.Serializable(0xB4B1868FEAA8FF2F, 0x4)]
        public enum ETextHAlignment : int32
        {
            _0 = 0,
            left = 1,
            center = 2,
            right = 3,
        }

        [RTTI.Serializable(0x9EF37482788CAD00, 0x4)]
        public enum ETextOrientation : int32
        {
            _0 = 0,
            tl_br = 1,
            bl_tr = 2,
            tr_bl = 3,
        }

        [RTTI.Serializable(0x72D17C5609D53967, 0x4)]
        public enum ETextOverflow : int32
        {
            _0 = 0,
            visible = 1,
            hidden = 2,
            scroll = 3,
            truncate = 4,
            scaledown = 5,
        }

        [RTTI.Serializable(0xAD05169049168F6C, 0x4)]
        public enum ETextTransform : int32
        {
            _0 = 0,
            none = 1,
            capitalize = 4,
            lowercase = 3,
            uppercase = 2,
        }

        [RTTI.Serializable(0xADDDA3B998A4A49C, 0x4)]
        public enum ETextWhiteSpace : int32
        {
            _0 = 0,
            normal = 1,
            nowrap = 2,
        }

        [RTTI.Serializable(0xBDB6122A54A9390A, 0x4)]
        public enum ETextureChannel : int32
        {
            R = 0,
            G = 1,
            B = 2,
            A = 3,
            Constant0 = 4,
            Constant1 = 5,
            RGB = 6,
            All = 7,
        }

        [RTTI.Serializable(0x9ABBCD72A39CBDA8, 0x4)]
        public enum ETextureRepeat : int32
        {
            _0 = 0,
            no_repeat = 1,
            repeat_x = 2,
            repeat_y = 3,
            repeat = 4,
        }

        [RTTI.Serializable(0x2104D12D3FDD3631, 0x4)]
        public enum ETextureSetQualityType : int32
        {
            Default = 0,
            Compressed_High = 1,
            Compressed_Low = 2,
            Uncompressed = 3,
            Normal_BC6 = 4,
            Normal_High = 5,
            Normal_Low = 6,
            BC4 = 8,
            Clean = 7,
            NormalRoughnessBC7 = 9,
            AlphaToCoverageBC4 = 10,
            Count = 11,
        }

        [RTTI.Serializable(0x3AF1180492C43999, 0x4)]
        public enum ETextureSetStorageType : int32
        {
            RGB = 0,
            R = 1,
            G = 2,
            B = 3,
            A = 4,
            Count = 5,
        }

        [RTTI.Serializable(0x8DFC622102B37A14, 0x4)]
        public enum ETextureSetType : int32
        {
            Invalid = 0,
            Color = 1,
            Alpha = 2,
            Normal = 3,
            Reflectance = 4,
            AO = 5,
            Roughness = 6,
            Height = 7,
            Mask = 8,
            Mask_Alpha = 9,
            Incandescence = 10,
            Translucency_Diffusion = 11,
            Translucency_Amount = 12,
            Misc_01 = 13,
            Count = 14,
        }

        [RTTI.Serializable(0x85B079E963333F06, 0x4)]
        public enum EThirdPersonCameraActorState : int32
        {
            None = 0,
            Bind = 1,
            In = 2,
            Full = 3,
            Out = 4,
            Done = 5,
            Wait = 6,
            Unbind = 7,
        }

        [RTTI.Serializable(0x60A100CE5630C1A4, 0x4)]
        public enum EThreatState : int32
        {
            none = -1,
            presence_undetected = 0,
            presence_suspected = 1,
            presence_confirmed = 2,
            threat_identified = 3,
        }

        [RTTI.Serializable(0x72CC04F8E9E6363F, 0x1)]
        public enum ETickerAlignment : int8
        {
            AlignTop = 0,
            AlignBottom = 1,
        }

        [RTTI.Serializable(0xAA39BA8739CBDB9D, 0x4)]
        public enum ETimerStartType : int32
        {
            Cooked = 0,
            OnEject = 1,
            OnImpact = 2,
        }

        [RTTI.Serializable(0xE0390D415171CFDF, 0x1)]
        public enum EToReassignRoleType : int8
        {
            same_as_original = 0,
            essential = 1,
            optional = 2,
            fictive = 3,
        }

        [RTTI.Serializable(0xE27A72B6C583140E, 0x1)]
        public enum ETrackingPathUpBlendType : int8
        {
            TerrainToLocalUp = 0,
            TerrainToPathUp = 1,
            PathToLocalUp = 2,
        }

        [RTTI.Serializable(0x4FB18334784FA96C, 0x4)]
        public enum ETrajectorySolveMethod : int32
        {
            Iterative = 0,
            Linear = 1,
            TwoPhaseRockets = 2,
        }

        [RTTI.Serializable(0xF9F506B5D1718FCF, 0x1)]
        public enum ETranslationStatus : int8
        {
            NotApproved = 0,
            TranslationApproved = 1,
            QADBApproved = 2,
            QAApproved = 3,
        }

        [RTTI.Serializable(0xAC48E47BE263FF7A, 0x4)]
        public enum ETriState : int32
        {
            False = 0,
            True = 1,
            Default = -1,
        }

        [RTTI.Serializable(0x55639246024C0A7D, 0x4)]
        public enum ETriggerExposedActionReplication : int32
        {
            ALL_CLIENTS_IF_NETOWNER = 0,
            ALL_CLIENTS = 1,
        }

        [RTTI.Serializable(0x326011A7EF492B05, 0x4)]
        public enum ETriggerType : int32
        {
            Press = 0,
            Release = 1,
            Continuous = 2,
            Hold = 3,
            Hold_Once = 4,
            Release_NoHold = 5,
            None = 6,
        }

        [RTTI.Serializable(0x738D185BE6064B58, 0x1)]
        public enum EUseLocationSelectionSortType : int8
        {
            CenterScreen = 0,
            UserOrientation = 1,
        }

        [RTTI.Serializable(0xD4AD115365F588E5, 0x4)]
        public enum EUseLocationType : int32
        {
            General = 0,
            WeaponPickup = 1,
            AutoPickup = 2,
            AmmoPickup = 3,
        }

        [RTTI.Serializable(0x823A2A3667040242, 0x4)]
        public enum EVaultEndInParkourAnnotationDirection : int32
        {
            Parallel = 0,
            Perpendicular = 1,
        }

        [RTTI.Serializable(0x6703FBF3B374181, 0x4)]
        public enum EVaultEndInParkourType : int32
        {
            On_Foot_Point = 0,
            On_Foot_Bar = 1,
            Hanging_With_FootSupport = 2,
            Hanging_Without_FootSupport = 3,
        }

        [RTTI.Serializable(0xDCE4FB3364B5D53F, 0x4)]
        public enum EVaultObstacleType : int32
        {
            Invalid = -1,
            Vertical = 0,
            Horizontal = 1,
            Parkour = 2,
        }

        [RTTI.Serializable(0xFC42E9F2366039E2, 0x4)]
        public enum EVaultType : int32
        {
            Not_Set = -1,
            Step_Over = 0,
            Step_Up = 1,
            Step_Off = 2,
        }

        [RTTI.Serializable(0xDF41686E0FC76D2B, 0x1)]
        public enum EVertexElement : int8
        {
            Pos = 0,
            TangentBFlip = 1,
            Tangent = 2,
            Binormal = 3,
            Normal = 4,
            Color = 5,
            UV0 = 6,
            UV1 = 7,
            UV2 = 8,
            UV3 = 9,
            UV4 = 10,
            UV5 = 11,
            UV6 = 12,
            MotionVec = 13,
            Vec4Byte0 = 14,
            Vec4Byte1 = 15,
            BlendWeights = 16,
            BlendIndices = 17,
            BlendWeights2 = 18,
            BlendIndices2 = 19,
            PivotPoint = 20,
            AltPos = 21,
            AltTangent = 22,
            AltBinormal = 23,
            AltNormal = 24,
            AltColor = 25,
            AltUV0 = 26,
            Invalid = 27,
        }

        [RTTI.Serializable(0xA38F534E13232B36, 0x4)]
        public enum EVerticalAlignment : int32
        {
            _0 = 0,
            baseline = 1,
            top = 2,
            middle = 3,
            bottom = 4,
            text_bottom = 5,
        }

        [RTTI.Serializable(0x70727F5D42D174, 0x4)]
        public enum EViewLayer : int32
        {
            Background = 0,
            Default = 1,
            FirstPerson = 2,
            Overlay = 3,
        }

        [RTTI.Serializable(0x45565FE06D55D411, 0x4)]
        public enum EVoiceLimitMode : int32
        {
            None = 0,
            Stop_Oldest = 1,
            Reject_New = 2,
        }

        [RTTI.Serializable(0x5A541F77CFC4071A, 0x4)]
        public enum EVolumetricAnnotationGroup : int32
        {
            None = 0,
            AI_Vision = 1,
            AI_Melee = 2,
            AI_Other = 3,
        }

        [RTTI.Serializable(0xBE8BBEF8204CCD33, 0x4)]
        public enum EWarpedAnimationActions : int32
        {
            Trigger_at_start = 0,
            Keep_active = 1,
        }

        [RTTI.Serializable(0x8BD5C28B790EE88D, 0x4)]
        public enum EWarpedAnimationDynamicVariableSource : int32
        {
            Rotation_Heading = 0,
            Translation_X = 1,
            Translation_Y = 2,
            Translation_Z = 3,
        }

        [RTTI.Serializable(0x825353747EFE78B, 0x4)]
        public enum EWaveDataEncoding : int32
        {
            PCM = 0,
            PCM_FLOAT = 1,
            XWMA = 2,
            ATRAC9 = 3,
            MP3 = 4,
            ADPCM = 5,
            AAC = 6,
        }

        [RTTI.Serializable(0x15B118418D20EE5E, 0x4)]
        public enum EWaveDataEncodingHint : int32
        {
            ATRAC9 = 3,
            MP3 = 4,
            AAC = 6,
            Auto_Select = 7,
        }

        [RTTI.Serializable(0xEACB83E079B3B70E, 0x4)]
        public enum EWaveDataEncodingQuality : int32
        {
            Uncompressed__PCM_ = 0,
            Lossy_Lowest = 1,
            Lossy_Low = 2,
            Lossy_Medium = 3,
            Lossy_High = 4,
            Lossy_Highest = 5,
        }

        [RTTI.Serializable(0xC27CC0FE81479C22, 0x4)]
        public enum EWeaponStanceRaiseType : int32
        {
            Never = 0,
            Raise_on_start_aim = 1,
            Raise_on_fire = 2,
        }

        [RTTI.Serializable(0x255D9637E064B03C, 0x4)]
        public enum EWeaponTriggerType : int32
        {
            Full_Auto = 0,
            Single_Shot_on_Press = 1,
            Single_Shot_on_Release = 2,
        }

        [RTTI.Serializable(0x6B6AD790E79BE1D, 0x4)]
        public enum EWidgetLayer : int32
        {
            _0 = 0,
            pre_shader = 1,
            post_shader = 2,
        }

        [RTTI.Serializable(0x12DB317940E2314F, 0x4)]
        public enum EWorldDataAccessMode : int32
        {
            Access_By_CPU_Only = 1,
            Access_By_GPU_Only = 2,
            Access_By_CPU_And_GPU = 3,
        }

        [RTTI.Serializable(0xEB620596A84FBC21, 0x4)]
        public enum EWorldDataBakeBlendMode : int32
        {
            None = 0,
            Alpha = 1,
            Additive = 2,
            Max = 3,
        }

        [RTTI.Serializable(0x4636C54A79A22431, 0x4)]
        public enum EWorldDataDecodingMode : int32
        {
            Default_Decoding = 0,
            NormalMap_Decoding = 1,
        }

        [RTTI.Serializable(0x6683089E995C1C46, 0x4)]
        public enum EWorldDataInputLayerApplyMode : int32
        {
            Absolute = 0,
            Relative = 1,
        }

        [RTTI.Serializable(0xD8E0DA88FF61F07A, 0x4)]
        public enum EWorldDataRttiType : int32
        {
            _float = 0,
            _HalfFloat = 1,
            _Vec4 = 2,
            _uint32 = 3,
            _uint16 = 4,
            _uint8 = 5,
            _RGBAColorRev = 6,
            _FRGBAColor = 7,
        }

        [RTTI.Serializable(0xFFC35B5BB713090F, 0x4)]
        public enum EWorldDataSourceDataMode : int32
        {
            ImageData = 0,
            Generated = 1,
            Baked = 2,
            Painted = 3,
        }

        [RTTI.Serializable(0xBBA39DCB2B83EF00, 0x4)]
        public enum EWorldDataTileBorderMode : int32
        {
            Untouched = 0,
            Average = 1,
        }

        [RTTI.Serializable(0xC1BB51F2450FCCC3, 0x1)]
        public enum EXpBarVisibility : int8
        {
            Always = 0,
            WhenNonZero = 1,
            OnlyOnEvent = 2,
        }

        [RTTI.Serializable(0x53AEF5FDB07DFCB8, 0x4)]
        public enum EventHandlerNeedsUpdate : int32
        {
            Auto = 0,
            Never = 1,
            Always = 2,
        }

        [RTTI.Serializable(0x3E1A2D264138ECB9, 0x4)]
        public enum SpawnAlgorithmType : int32
        {
            DensityWeight = 0,
            LeastDensity = 1,
            NeedMostEntities = 2,
        }

    }
}
