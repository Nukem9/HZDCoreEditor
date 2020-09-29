#pragma warning disable CS0649 // warning CS0649: 'member' is never assigned to, and will always have its default value 'value'.
#pragma warning disable CS0108 // warning CS0108: 'class' hides inherited member 'member'. Use the new keyword if hiding was intended.

namespace Decima.DS
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
    using ucs4 = System.Int32;

    using HalfFloat = System.UInt16;
    using LinearGainFloat = System.Single;
    using MusicTime = System.UInt64;

    using MaterialType = System.UInt16;
    using AnimationNodeID = System.UInt16;
    using AnimationTagID = System.UInt32;
    using AnimationSet = System.UInt32;
    using AnimationStateID = System.UInt32;
    using AnimationEventID = System.UInt32;
    using PhysicsCollisionFilterInfo = System.UInt32;

    [RTTI.Serializable(0x860E4B096602077A, GameType.DS)]
    public enum AnimationMountStateLogic : int8
    {
        Tag = 0,
        Event = 1,
    }

    [RTTI.Serializable(0x4048C886DA6FDBFA, GameType.DS)]
    public enum ClanRole : int32
    {
        BASIC = 0,
        OFFICER = 1,
    }

    [RTTI.Serializable(0x789431BD7D118A93, GameType.DS)]
    public enum ClanStatus : int32
    {
        OK = 0,
        ERROR = 1,
    }

    [RTTI.Serializable(0xEE598C14069D7173, GameType.DS)]
    public enum DSControlledNPCPositionMarker_NPC : int8
    {
        Fragile = 0,
        Mama = 1,
        DeadMan = 2,
        Lockne = 3,
        Tower = 4,
    }

    [RTTI.Serializable(0xFF3C8EAD843F683D, GameType.DS)]
    public enum DSMissionEventTimerDisplayMethod : int8
    {
        None = 0,
        TimerHudCountDown = 1,
        TimerHudCountUp = 2,
    }

    [RTTI.Serializable(0x81021D7C336570DB, GameType.DS)]
    public enum EAAMode : int32
    {
        None = 0,
        FXAA = 1,
        TAA = 2,
        Default = -1,
    }

    [RTTI.Serializable(0xBCBCB1FC7B8BC38E, GameType.DS)]
    public enum EAIAttackType : int32
    {
        Area = 3,
        Ballistic = 2,
        Contact = 0,
        Line = 1,
    }

    [RTTI.Serializable(0x59B105389B2BDB6, GameType.DS)]
    public enum EAIBehaviorGroupMemberNavmeshPlacmentType : int8
    {
        FindRandomPointInRangeOnNavmesh = 0,
        FindRandomPointInRangeInAirNav = 1,
    }

    [RTTI.Serializable(0x97E543FE36E90C98, GameType.DS)]
    public enum EAIBodyAlignmentMode : int8
    {
        TurnUsingAnimation = 0,
        TurnWithoutAnimation = 1,
        NoTurnWhileOperating = 2,
    }

    [RTTI.Serializable(0x8C8262D1BF2F10D9, GameType.DS)]
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

    [RTTI.Serializable(0x71A853F25377910E, GameType.DS)]
    public enum EAIPatrolPathType : int32
    {
        Loop = 0,
        Once = 1,
        BackForth = 2,
        BackForthOnce = 3,
    }

    [RTTI.Serializable(0x463EA09673894D77, GameType.DS)]
    public enum EAIRoadUsableBy : int8
    {
        None = 0,
        Humans = 1,
        Robots = 2,
        Player = 4,
        All = 7,
    }

    [RTTI.Serializable(0x962FD3E9FAC0FD00, GameType.DS)]
    public enum EActivateConditionRelation : int8
    {
        And = 0,
        Or = 1,
        Override = 2,
    }

    [RTTI.Serializable(0xECAB83E9A7F337A0, GameType.DS)]
    public enum EActivationType : int32
    {
        Normal = 0,
        Exclusive = 1,
    }

    [RTTI.Serializable(0x13E84224B65188AC, GameType.DS)]
    public enum EActiveView : int32
    {
        None = -1,
        Default = 0,
        ThirdPerson = 1,
        FirstPerson = 2,
    }

    [RTTI.Serializable(0x85E4B88E07781A6C, GameType.DS)]
    public enum EActivityMedalType : int32
    {
        Golden = 3,
        Silver = 2,
        Bronze = 1,
    }

    [RTTI.Serializable(0x3FE9099AA4CBF5C7, GameType.DS)]
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

    [RTTI.Serializable(0x5616D30C32DB4214, GameType.DS)]
    public enum EAlertPartType : int32
    {
        Alert = 0,
        Array = 1,
        Text = 2,
        FieldList = 4,
        Field = 5,
        RTTIObject = 6,
    }

    [RTTI.Serializable(0x9A5BF5EE6FA9DCBB, GameType.DS)]
    public enum EAlertType : int32
    {
        Normal = 0,
        TerminateProcess = 1,
        LogOnly = 2,
    }

    [RTTI.Serializable(0x36F48A22F8EBDD81, GameType.DS)]
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

    [RTTI.Serializable(0x427FAED60FCC9B20, GameType.DS)]
    public enum EAmmoCostType : int32
    {
        Ammo_Per_Shot = 0,
        Ammo_Per_Burst = 1,
    }

    [RTTI.Serializable(0x440BE23BC83853D1, GameType.DS)]
    public enum EAmmoSettings : int32
    {
        AmmoLow = 0,
        AmmoNormal = 1,
        AmmoHigh = 2,
    }

    [RTTI.Serializable(0x2E89FD2E3826E7C1, GameType.DS)]
    public enum EAmmoTetherState : int8
    {
        Invalid_Tether_State = -1,
        Untethered__Idle = 0,
        Untethered__Searching = 1,
        Tethered__Loading = 2,
        Tethered__Idle = 3,
    }

    [RTTI.Serializable(0xBFAA0791E2E333E2, GameType.DS)]
    public enum EAnimationActionAction : int32
    {
        Start = 0,
        Stop = 1,
        Trigger = 2,
    }

    [RTTI.Serializable(0x347F8FCE8794808D, GameType.DS)]
    public enum EAnimationDamageType : int32
    {
        none = -1,
        projectile = 0,
        explosion = 2,
        fire = 1,
        electricity = 3,
    }

    [RTTI.Serializable(0x532D9C9E326793FF, GameType.DS)]
    public enum EAnimationDirection : int32
    {
        any = -1,
        front = 0,
        back = 1,
    }

    [RTTI.Serializable(0x93905C311BFB983B, GameType.DS)]
    public enum EAnimationTransitionCollisionPath : int8
    {
        None = 0,
        FromAnimationEvents = 1,
        Automatic = 2,
    }

    [RTTI.Serializable(0x477CB10157655881, GameType.DS)]
    public enum EAnimationTriggerType : int8
    {
        AnimationEvent = 0,
        AnimationTag = 1,
    }

    [RTTI.Serializable(0x47367503C856B39E, GameType.DS)]
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

    [RTTI.Serializable(0x102D6E8F62A8B693, GameType.DS)]
    public enum EApertureShape : int32
    {
        Polygon = 0,
        Circle = 1,
        Texture = 2,
    }

    [RTTI.Serializable(0x6F43EC706669C28B, GameType.DS)]
    public enum EArcTargetType : int32
    {
        None = 0,
        Entity = 1,
        World = 2,
        Air = 3,
    }

    [RTTI.Serializable(0xCE4484D060B20C7, GameType.DS)]
    public enum EAreaTargetShapeType : int8
    {
        Invalid = 0,
        Sphere = 1,
        Box = 2,
        Capsule = 3,
        ShapeCurve = 4,
    }

    [RTTI.Serializable(0x4AFAA90C138C2916, GameType.DS)]
    public enum EArtPartsSubModelKind : int32
    {
        None = 0,
        Cover = 1,
        CoverAndAnim = 2,
    }

    [RTTI.Serializable(0x140F38B1A09A055A, GameType.DS)]
    public enum EAttackEventLinkType : int8
    {
        Invalid = 0,
        CreateNewChain = 1,
        DirectConsequence = 2,
        IndirectConsequence = 3,
        PassThrough = 4,
    }

    [RTTI.Serializable(0x7D8C04C9EBD402B, GameType.DS)]
    public enum EAttackEventSource : int8
    {
        Invalid = 0,
        CreateNewChain = 1,
        ActiveWeapon = 2,
        Entity = 3,
        NodeGraph = 4,
    }

    [RTTI.Serializable(0xFCF4D7881044CFA4, GameType.DS)]
    public enum EAttackEventType : int8
    {
        Unspecified = 0,
        DeliberatelyEmpty = 1,
        SelfInflicted = 2,
        Drowning = 3,
        Environmental = 4,
        Physics = 5,
        SequenceEvent = 6,
        AttackEvent = 7,
        WeaponBurst = 8,
        MeleeAttack = 9,
        Explosion = 10,
        DamageArea = 11,
        ImpactDamage = 12,
        GraphNode = 13,
    }

    [RTTI.Serializable(0x54CC71356FD97E32, GameType.DS)]
    public enum EAttackNodePolicy : int8
    {
        AttackRoot = 0,
        AttackHighest = 1,
    }

    [RTTI.Serializable(0x1098E9129E984E05, GameType.DS)]
    public enum EAwarenessType : int32
    {
        Unaware = 0,
        Identified = 1,
        Suspected = 2,
    }

    [RTTI.Serializable(0x6E49E82C3DCB73A8, GameType.DS)]
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

    [RTTI.Serializable(0xEC4A2415DFFFF2D4, GameType.DS)]
    public enum EBehaviourOnHide : int32
    {
        Success = 0,
        Fail = 1,
        Hide = 2,
    }

    [RTTI.Serializable(0x425E6E40265B37A4, GameType.DS)]
    public enum EBlend2SyncPassThrough : int8
    {
        Source1 = 0,
        Source2 = 1,
    }

    [RTTI.Serializable(0xEB5020F74775B470, GameType.DS)]
    public enum EBlendEventPropagationMode : int8
    {
        None = 0,
        Source0 = 1,
        Source1 = 2,
        Merge = 3,
    }

    [RTTI.Serializable(0x9A039528856562D4, GameType.DS)]
    public enum EBlendMode : int8
    {
        Interpolate = 0,
        Add = 1,
        Substract = 2,
    }

    [RTTI.Serializable(0xCB486D8385A352E4, GameType.DS)]
    public enum EBuddySpawnRequestMode : int8
    {
        None = 0,
        SpawnMarker = 1,
        Spawnpoint = 2,
        LastKnownPosition = 3,
        NearPlayer = 4,
    }

    [RTTI.Serializable(0x1A83801E42D51EA4, GameType.DS)]
    public enum EBuddyState : int8
    {
        Unregistered = 0,
        Inactive = 1,
        Spawning = 2,
        Active = 3,
        Dead = 4,
        Despawned = 5,
    }

    [RTTI.Serializable(0x347508E78F36221, GameType.DS)]
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
    }

    [RTTI.Serializable(0x165743D26B190DC3, GameType.DS)]
    public enum ECameraFollowMode : int8
    {
        None = 0,
        OriginForward = 1,
        LinearVelocity = 2,
    }

    [RTTI.Serializable(0x1190F96FDF0BF07A, GameType.DS)]
    public enum ECameraModeComparator : int8
    {
        Previous = 0,
        Current = 1,
    }

    [RTTI.Serializable(0xF371DA5B2488A6A0, GameType.DS)]
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

    [RTTI.Serializable(0x5F21A55D24C37829, GameType.DS)]
    public enum ECameraTransitionFunction : int32
    {
        TransitionLinear = 0,
        TransitionSmoothStep = 1,
    }

    [RTTI.Serializable(0xF167EEF7E60B3668, GameType.DS)]
    public enum ECanActivateWhileSwimming : int8
    {
        No = 0,
        Yes = 1,
        YesExclusively = 2,
    }

    [RTTI.Serializable(0x8F464FA4505EF3A0, GameType.DS)]
    public enum ECarryModes : int32
    {
        INVALID = -1,
        IDLE = 0,
        TACTICAL = 1,
        COMBAT = 2,
    }

    [RTTI.Serializable(0xD164F73274D68405, GameType.DS)]
    public enum ECartEventSearchType : int32
    {
        None = 0,
        Cart = 1,
    }

    [RTTI.Serializable(0xCE25EA36C86B63C3, GameType.DS)]
    public enum EChildrenClipMode : int32
    {
        _0 = 0,
        clip = 1,
        noclip = 2,
    }

    [RTTI.Serializable(0x8BA079E7C0C202AC, GameType.DS)]
    public enum EChiralParticleRenderingMode : int8
    {
        Background = 0,
        HalfRes = 1,
        Foreground = 2,
        Deferred = 3,
        DeferredAndDepthOnly = 4,
        BackgroundAndStencil = 5,
    }

    [RTTI.Serializable(0x1032479AB3F9E6B9, GameType.DS)]
    public enum EClanMatchOutcome : int8
    {
        FACTION_1_WON = 0,
        FACTION_2_WON = 1,
        DRAW = 2,
        NO_GAME = 3,
    }

    [RTTI.Serializable(0xF7C1F43367315158, GameType.DS)]
    public enum ECliffVoicePowerFlag : int8
    {
        None = 0,
        Quiet = 1,
        Normal = 2,
        Shout = 4,
        Any = 7,
    }

    [RTTI.Serializable(0xEF96D0DE01B041E1, GameType.DS)]
    public enum ECliffVoiceSituationType : int32
    {
        None = 0,
        BattleCry = 1,
        BattleCry_Strong = 2,
        FirstPunchDamage = 3,
        SecondPunchDamage = 4,
        KickDamage = 5,
        NonLethalWeapon = 6,
        NonLethalSmoke = 7,
        ResistanceDamage = 8,
        HeavyDamage = 9,
        ResistingHeavyDamage = 10,
        Scream = 11,
        ExGrenade = 12,
        ElectricalDamage = 13,
        Tied = 14,
        AfterTied = 15,
        HelmetKnockedOff = 16,
        Slipping = 17,
        MortalAgony = 18,
        Order_Attack = 19,
        Order_DoIt = 20,
        Order_Grenade = 21,
        Order_Hide = 22,
        Order_Recommence = 23,
        Order_Retreat = 24,
        Order_Disengage = 25,
        Order_Flank = 26,
        Order_Forward = 27,
        Order_Hold = 28,
        Order_Pursuit = 29,
        Order_Rally = 30,
        Order_Search = 31,
        Order_Scatter = 32,
        Order_Smoke = 33,
        Order_Tar_Retreat = 34,
        Order_Bazooka = 35,
        Order_LieDown = 60,
        Caution = 36,
        Grenade = 37,
        BattleStart = 38,
        Reinforcements = 39,
        ReinforcementsBell = 40,
        AngryRoar = 41,
        NoticesSomething_NotScared = 42,
        NoticesSomething = 43,
        NoticesSomething_Scary = 44,
        Surprise = 45,
        BB_Related = 46,
        BB_RelatedWhenIdle = 47,
        Cigarette = 48,
        InappropriateLaughter = 49,
        EchoReply = 50,
        Breathing = 51,
        Praise = 52,
        GetUp = 53,
        KickAttack = 54,
        MortalAgony_Vietnam_B = 55,
        MortalAgony_Vietnam_F = 56,
        CordAttack = 57,
        Dying = 58,
        Sniped = 59,
        PlayerDeath = 61,
        Uncategorized = 62,
        MAX = 63,
    }

    [RTTI.Serializable(0x831C935A35F332B, GameType.DS)]
    public enum ECliffVoiceTargetStageFlag : int8
    {
        None = 0,
        WW1 = 1,
        WW2 = 2,
        Vietnam = 4,
        All = 7,
    }

    [RTTI.Serializable(0x70FA52D57763BF45, GameType.DS)]
    public enum ECliffVoiceWarriorExistFlag : int8
    {
        None = 0,
        WW1 = 1,
        WW2 = 2,
        Any = 3,
    }

    [RTTI.Serializable(0x249D27C69B4F9330, GameType.DS)]
    public enum ECloseCombatSettings : int32
    {
        CloseCombatOn = 0,
        CloseCombatOff = 1,
    }

    [RTTI.Serializable(0xCA94E62A6BC89319, GameType.DS)]
    public enum EClosestAnimAlignmentType : int8
    {
        None = 0,
        Rotation = 1,
        RotationAndTranslation = 2,
    }

    [RTTI.Serializable(0xCAD6BCF3F85B57D1, GameType.DS)]
    public enum ECloudShaderType : int8
    {
        Guerrilla = 0,
        KJP = 1,
        KJP2 = 2,
        PassThrough = 3,
    }

    [RTTI.Serializable(0xE1D2D4678AE10A66, GameType.DS)]
    public enum EColorizeBlendMode : int32
    {
        Lerp = 0,
        ColorCorrect = 1,
    }

    [RTTI.Serializable(0xF9444CB3CD8A13F5, GameType.DS)]
    public enum EColorizeLookupMasterMode : int32
    {
        Default = 0,
        Black = 1,
        White = 2,
    }

    [RTTI.Serializable(0x319D1E5C1BDD4364, GameType.DS)]
    public enum ECommandPriority : int32
    {
        unspecified = 0,
        blind_following = 1,
        follow_orders = 2,
        non_battle_initiative = 3,
        idle = 4,
    }

    [RTTI.Serializable(0x54ADABAA2A86058E, GameType.DS)]
    public enum EComparator : int32
    {
        Equals = 0,
        NotEquals = 1,
        GreaterThan = 2,
        GreaterThanEquals = 3,
        LessThan = 4,
        LessThanEquals = 5,
    }

    [RTTI.Serializable(0x5B36ABC4939560FE, GameType.DS)]
    public enum ECompletionAutoRotate : int32
    {
        None = 0,
        UseObjectRotation = 1,
        RotateToLight = 2,
        RotateToCentre = 3,
    }

    [RTTI.Serializable(0xEFF1F84E194DBE8B, GameType.DS)]
    public enum EComputeThreadDistribution : int32
    {
        MaxThreads_1D = 0,
        MaxThreads_2D = 1,
        MaxThreads_3D = 2,
    }

    [RTTI.Serializable(0xDC68E3C1D8729D46, GameType.DS)]
    public enum EConstraintComponentAttachmentTarget : int8
    {
        Self = 0,
        Parent = 1,
        World = 2,
    }

    [RTTI.Serializable(0x618022182A056D6A, GameType.DS)]
    public enum EContactType : int32
    {
        Colliding_and_resting = 0,
        Colliding_and_bouncing = 1,
        Sliding = 2,
        Rolling = 3,
    }

    [RTTI.Serializable(0xEB3BBFB950180268, GameType.DS)]
    public enum EContextualActionAnimationActions : int8
    {
        Trigger_at_start = 0,
        Keep_active = 1,
    }

    [RTTI.Serializable(0xB70BBDE506C8C3D4, GameType.DS)]
    public enum EContextualActionButtonType : int8
    {
        Single_button_press = 0,
        Continuous_button_press = 1,
    }

    [RTTI.Serializable(0xDF83412C91BAAE4C, GameType.DS)]
    public enum EContextualActionDeviceFunctionType : int8
    {
        PrimaryContextualAction = 0,
        SecondaryContextualAction = 1,
        TertiaryContextualAction = 2,
    }

    [RTTI.Serializable(0x1E9461F7EFCB00B4, GameType.DS)]
    public enum EContextualActionSwitchToWeapon : int8
    {
        Switch_to_MeleeWeapon = 0,
        Switch_to_Nothing = 1,
    }

    [RTTI.Serializable(0xD61AF03774CC5572, GameType.DS)]
    public enum EContextualActionTriggerAction : int8
    {
        Trigger_at_start = 0,
        Trigger_at_event = 1,
        Trigger_on_mount = 2,
    }

    [RTTI.Serializable(0x7157E789D46864B, GameType.DS)]
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

    [RTTI.Serializable(0xA9AD31C8735D73F6, GameType.DS)]
    public enum EControllerScheme : int32
    {
        Dual_Shock = 0,
        Remote_Play = 1,
    }

    [RTTI.Serializable(0xA3DC8D326D0794A4, GameType.DS)]
    public enum ECreateAsChild : int32
    {
        If_mover_requires_parent = 0,
        Autonomous_child = 1,
        Child_owned_by_parent = 2,
    }

    [RTTI.Serializable(0x38E4279787DF9832, GameType.DS)]
    public enum ECreateEntityFactionOverride : int32
    {
        None = 0,
        Entity = 1,
        Activator = 2,
        Instigator = 3,
    }

    [RTTI.Serializable(0xFF5F1533428EA2CC, GameType.DS)]
    public enum ECreateEntityLifetime : int32
    {
        Automatic = 0,
        Scene = 1,
        OtherEntity = 2,
        Dispensable = 3,
        Manual = 4,
    }

    [RTTI.Serializable(0xC0D76DF4E8676FD7, GameType.DS)]
    public enum ECrosshairBulletIndicatorType : int8
    {
        Invalid = 0,
        MagazineSize = 1,
        BurstSize = 2,
    }

    [RTTI.Serializable(0xEF4AAE849AB82235, GameType.DS)]
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
        PreFire = 9,
        Fire = 10,
        PerfectFire = 11,
        TargetAquired = 12,
        TargetLost = 13,
        OutOfRange = 14,
        InRange = 15,
    }

    [RTTI.Serializable(0x4C91C3DAFB67B482, GameType.DS)]
    public enum ECrowdImpostorAnimationState : int8
    {
        Walk = 0,
        Stand = 1,
        Sit = 2,
        Crouch = 3,
    }

    [RTTI.Serializable(0x1088866198E83A92, GameType.DS)]
    public enum ECubemapZoneDataStorageMode : int8
    {
        NonStreamingData = 0,
        StreamingData = 1,
    }

    [RTTI.Serializable(0xE440A7BF5092B8C3, GameType.DS)]
    public enum ECull : int32
    {
        CW = 1,
        CCW = 2,
        Off = 0,
    }

    [RTTI.Serializable(0xCA90811B051840FB, GameType.DS)]
    public enum ECurveEvaluationType : int8
    {
        Hermite = 0,
        Bezier = 1,
    }

    [RTTI.Serializable(0x66194B08850D65FD, GameType.DS)]
    public enum EDSAICoverType : int32
    {
        HighCover = 0,
        MediumCover = 1,
        LowCover = 2,
        NoCover = 3,
    }

    [RTTI.Serializable(0x80E441C0B2CE2403, GameType.DS)]
    public enum EDSAmmoCategory : int8
    {
        None = 0,
        AssaultRifle = 1,
        ShotGun = 2,
        HandGun = 3,
        RubberAssaultRifle = 4,
        RubberShotGun = 5,
        GrenadeShell = 6,
        BloodGrenadeShell = 7,
        SlipGrenadeShell = 8,
        AmnioticFluidGrenadeShell = 9,
        TranquilizerGrenadeShell = 10,
        SubGrenadeShell = 11,
        SubBloodGrenadeShell = 12,
        SubSlipGrenadeShell = 13,
        SubAmnioticFluidGrenadeShell = 14,
        SubTranquilizerGrenadeShell = 15,
        BolaGun = 16,
    }

    [RTTI.Serializable(0x8B56BA9375AA385B, GameType.DS)]
    public enum EDSAmmoId : int8
    {
        None = 0,
        BloodBullet = 1,
        GoldenBullet = 2,
        AssaultRifle = 3,
        AssaultRifleLv2 = 4,
        AssaultRifleLv3 = 5,
        AssaultRifleLv4 = 6,
        Grenade = 7,
        BloodGrenade = 8,
        BloodGrenadeLv1Extend = 9,
        BloodGrenadeLv2 = 10,
        ElectricalGrenadeLv1 = 11,
        ElectricalGrenadeLv2 = 12,
        ElectricalGrenadePlace = 13,
        CoatingSpray = 14,
        SmokeGrenade = 15,
        SmokeGrenadeLv2 = 16,
        FreezeGrenade = 17,
        TranquilizerGun = 18,
        AmnioticFluidGrenade = 19,
        ExGrenade0 = 20,
        ExGrenade1 = 21,
        ExGrenade1Plus = 22,
        ExGrenade2 = 23,
        BolaGun = 24,
        BolaGunLv2 = 25,
        ShotGun = 26,
        ShotGunLv2 = 27,
        ShotGunLv3 = 28,
        HandGun = 29,
        HandGunLv2 = 30,
        HandGunLv3 = 31,
        BloodHandGun = 32,
        BloodHandGunLv2 = 33,
        AmelieHandGun = 34,
        C4 = 35,
        GazerBalloon = 36,
        SamBall = 37,
        SamBallLv2 = 38,
        GrenadeShell = 39,
        BloodGrenadeShell = 40,
        SlipGrenadeShell = 41,
        AmnioticFluidGrenadeShell = 42,
        TranquilizerGrenadeShell = 43,
        AssaultRifleRubberBullet = 44,
        AssaultRifleRubberBulletLv2 = 45,
        AssaultRifleRubberBulletLv3 = 46,
        AssaultRifleRubberBulletLv4 = 47,
        ShotGunRubberBullet = 48,
        ShotGunRubberBulletLv2 = 49,
        ShotGunRubberBulletLv3 = 50,
        Builder = 51,
        Ladder = 52,
        Rope = 53,
        StickyBullet = 54,
        FourConsecutiveMissile = 55,
        SpreadMissile = 56,
        HologramDevice = 57,
        EnemyAssaultRifle = 58,
        EnemyAssaultRifleRubberBullet = 59,
        HiggsAssaultRifleBullet = 60,
        Ww1Rifle = 61,
        Ww1ShotGun = 62,
        Ww1Grenade = 63,
        Ww1MachineGun = 64,
        Ww2SubmachineGun = 65,
        Ww2Rifle = 66,
        Ww2Missile = 67,
        Ww2MissileType2 = 68,
        Ww2SmokeGrenade = 69,
        VietnamAssault = 70,
        VietnamAssaultWithGrenader = 71,
        VietnamAssaultWithGrenaderShell = 72,
        VietnamMachinegun = 73,
        VietnamGrenade = 74,
        CliffRifle = 75,
        AfghanRifle = 76,
        DemensAssaultRifleBullet = 77,
        DemensShotGunBullet = 78,
        EnemyGrenade = 79,
        Ww2Grenade = 80,
        AfghanGrenade = 81,
        Ww2AirPlaneMachinegun = 82,
        Ww2HeavyMachinegun = 83,
        DemensElectricalGrenade = 84,
        __0 = 85,
    }

    [RTTI.Serializable(0xE572AD3C9D0ABCAE, GameType.DS)]
    public enum EDSAmmoType : int32
    {
        Unknown = -1,
        Rifle = 0,
        ShotGun = 1,
        HandGun = 2,
        GrenadeShell = 3,
    }

    [RTTI.Serializable(0xECF2C5BE67AE3266, GameType.DS)]
    public enum EDSArea : uint16
    {
        Area00 = 0,
        Area01 = 100,
        Area02 = 200,
        Area03 = 300,
        Area04 = 400,
        Warrior01 = 500,
        Warrior02 = 510,
        Warrior03 = 520,
        Beach01 = 600,
        Empty = 65535,
        __0 = 10000,
        __1 = 10001,
        __2 = 10002,
        __3 = 10003,
        __4 = 10100,
        __5 = 10101,
        __6 = 10103,
        __7 = 10104,
        __8 = 10200,
        __9 = 15001,
        __a = 15002,
        __b = 20000,
        __c = 30000,
        __d = 10004,
    }

    [RTTI.Serializable(0xECA4903C37C286E7, GameType.DS)]
    public enum EDSAreaChangeReason : int8
    {
        None = 0,
        FastTravel = 1,
        WWI = 2,
        WWII = 3,
        Vietnam = 4,
        MovingByShip = 5,
        Area02ToArea04 = 6,
        M620 = 7,
        M640 = 8,
        Beach = 9,
        M010ToM020 = 10,
        Nightmare = 11,
        MemoriesOfCliff = 12,
    }

    [RTTI.Serializable(0xC5C25E058EFDA170, GameType.DS)]
    public enum EDSAttackId : int16
    {
        None = 0,
        AssaultRifle = 1,
        AssaultRifleLv2 = 2,
        AssaultRifleLv3 = 3,
        AssaultRifleLv4 = 4,
        AssaultRifleBloodBullet = 5,
        AssaultRifleLv2BloodBullet = 6,
        AssaultRifleLv3BloodBullet = 7,
        AssaultRifleLv4BloodBullet = 8,
        AssaultRifleGoldenBullet = 9,
        AssaultRifleLv2GoldenBullet = 10,
        AssaultRifleLv3GoldenBullet = 11,
        AssaultRifleLv4GoldenBullet = 12,
        Grenade = 13,
        BloodGrenade = 14,
        BloodGrenadeLv1Extend = 15,
        BloodGrenadeLv2 = 16,
        ElectricalGrenadeLv1 = 17,
        ElectricalGrenadeLv2 = 18,
        ElectricalGrenadePlace = 19,
        CoatingSpray = 20,
        SmokeGrenade = 21,
        SmokeGrenadeLv2 = 22,
        FreezeGrenade = 23,
        TranquilizerGun = 24,
        AmnioticFluidGrenade = 25,
        ExGrenade0 = 26,
        ExGrenade1 = 27,
        ExGrenade1Plus = 28,
        ExGrenade2 = 29,
        BolaGun = 30,
        BolaGunLv2 = 31,
        ShotGun = 32,
        ShotGunLv2 = 33,
        ShotGunLv3 = 34,
        ShotGunBloodBullet = 35,
        ShotGunBloodBulletLv2 = 36,
        ShotGunBloodBulletLv3 = 37,
        ShotGunGoldenBulletLv3 = 38,
        HandGun = 39,
        HandGunLv2 = 40,
        HandGunLv3 = 41,
        HandGunBloodBullet = 42,
        HandGunBloodBulletLv2 = 43,
        HandGunBloodBulletLv3 = 44,
        HandGunGoldenBullet = 45,
        HandGunGoldenBulletLv2 = 46,
        HandGunGoldenBulletLv3 = 47,
        BloodHandGun = 48,
        BloodHandGunLv2 = 49,
        AmelieHandGun = 50,
        C4 = 51,
        GazerBalloon = 52,
        MultiRod = 53,
        SamBall = 54,
        SamBallLv2 = 55,
        GrenadeShell = 56,
        BloodGrenadeShell = 57,
        BloodGrenadeShellBloodSmoke = 58,
        SlipGrenadeShell = 59,
        AmnioticFluidGrenadeShell = 60,
        TranquilizerGrenadeShell = 61,
        AssaultRifleRubberBullet = 62,
        AssaultRifleRubberBulletLv2 = 63,
        AssaultRifleRubberBulletLv3 = 64,
        AssaultRifleRubberBulletLv4 = 65,
        AssaultRifleRubberBloodBullet = 66,
        AssaultRifleRubberBloodBulletLv2 = 67,
        AssaultRifleRubberBloodBulletLv3 = 68,
        AssaultRifleRubberBloodBulletLv4 = 69,
        AssaultRifleRubberGoldenBulletLv3 = 70,
        AssaultRifleRubberGoldenBulletLv4 = 71,
        ShotGunRubberBullet = 72,
        ShotGunRubberBulletLv2 = 73,
        ShotGunRubberBulletLv3 = 74,
        ShotGunRubberBloodBullet = 75,
        ShotGunRubberBloodBulletLv2 = 76,
        ShotGunRubberBloodBulletLv3 = 77,
        ShotGunRubberGoldenBulletLv3 = 78,
        StickyBullet = 79,
        FourConsecutiveMissile = 80,
        FourConsecutiveMissileBlood = 81,
        SpreadMissile = 82,
        SpreadMissileBlood = 83,
        SpreadMissileChild = 84,
        SpreadMissileChildBlood = 85,
        EnemyAssaultRifle = 86,
        EnemyAssaultRifleRubberBullet = 87,
        HiggsAssaultRifleBullet = 88,
        Ww1Rifle = 89,
        Ww1ShotGun = 90,
        Ww1Grenade = 91,
        Ww1MachineGun = 92,
        Ww2SubmachineGun = 93,
        Ww2Rifle = 94,
        Ww2Missile = 95,
        Ww2MissileType2 = 96,
        Ww2SmokeGrenade = 97,
        VietnamAssault = 98,
        VietnamAssaultWithGrenader = 99,
        VietnamAssaultWithGrenaderShell = 100,
        VietnamMachinegun = 101,
        VietnamGrenade = 102,
        CliffRifle = 103,
        AfghanRifle = 104,
        HiggsKnife = 105,
        DemensAssaultRifleBullet = 106,
        DemensShotGun = 107,
        EnemyGrenade = 108,
        Ww2Grenade = 109,
        AfghanGrenade = 110,
        PoisonGasShell = 111,
        __1 = 112,
        GrenadeBody = 257,
        BolagunStrong = 258,
        BolagunWeak = 259,
        BaggageAttack = 260,
        BaggageAttackBig = 261,
        BaggageThrow = 262,
        Urination = 263,
        BloodUrination = 264,
        MuleNearAttack = 265,
        GazerArmBlowAttack = 266,
        PlayerTackle = 267,
        PlayerPull = 268,
        PlayerPullBaggage = 269,
        PlayerPullHeavy = 270,
        PlayerSlip = 271,
        PlayerLanded = 272,
        PlayerLandedDump = 273,
        PlayerLandFailed = 274,
        PlayerLandDead = 275,
        PlayerFallHitWall = 276,
        PlayerFallStomp = 277,
        PlayerTumble = 278,
        PlayerComboA_0 = 279,
        PlayerComboA_1 = 280,
        PlayerComboA_2 = 281,
        PlayerGloveComboA_0 = 282,
        PlayerGloveComboA_1 = 283,
        PlayerGloveComboA_2 = 284,
        PlayerSliding = 285,
        PlayerLowKick = 286,
        PlayerStomp = 287,
        PlayerHiggsComboA_0 = 288,
        PlayerHiggsComboA_1 = 289,
        PlayerHiggsComboA_2 = 290,
        HiggsComboA_0 = 291,
        HiggsComboA_1 = 292,
        HiggsComboA_2 = 293,
        GazerDrag = 294,
        CatcherAnnihilationAttack = 295,
        CatcherFluidAttack = 296,
        CatcherSwingAttack = 297,
        CatcherSwingAttack2 = 298,
        CatcherSwingAttack3 = 299,
        CatcherSwingAttack4 = 300,
        CatcherSwingAttack5 = 301,
        CatcherQuadPunchSubordinate = 302,
        CatcherQuadPunchBoss = 303,
        CatcherQuadTailSubordinate = 304,
        CatcherQuadTailBoss = 305,
        CatcherQuadStampSubordinate = 306,
        CatcherQuadStampBoss = 307,
        CatcherQuadRushSubordinate = 308,
        CatcherQuadRushBoss = 309,
        CatcherQuadTarBeamSubordinate = 310,
        CatcherQuadTarBeamBoss = 311,
        CatcherQuadJogSubordinate = 312,
        CatcherQuadJogBoss = 313,
        CatcherJellyfishBlastNormal = 314,
        CatcherJellyfishBlastGold = 315,
        CatcherTitanTentacleHit = 316,
        CatcherTitanTentacleFailed = 317,
        CatcherTitanPush = 318,
        CatcherTitanMissile = 319,
        CatcherWhaleMissile = 320,
        CatcherWhaleBomb = 321,
        CatcherWhaleTarBeam = 322,
        CatcherWhaleTarBeam2 = 323,
        CatcherWhaleTarBeamEx = 324,
        CatcherWhaleTarBeamExExplosion = 325,
        CatcherWhaleBodyBlow = 326,
        CatcherWhaleBossCapture = 327,
        RainDamageToBaggage = 328,
        PhysicsImpactToBaggage = 329,
        FallImpactToBaggage = 330,
        SlipFromGround = 331,
        SlipStrongFromGround = 332,
        SlipGrenade = 333,
        SlipGrenadeStrong = 334,
        VehicleTackle = 335,
        VehicleTacklePlayer = 336,
        VehiclePreliminaryTackle = 337,
        VehicleWheelStamp = 338,
        VehicleExplosion = 339,
        PlayerRopeStrangle = 340,
        PlayerRopeStranglePreparation = 341,
        Push = 342,
        BlastWave = 343,
        PoisonGasShellExplosion = 344,
        PoisonGasShellBody = 345,
        PoisonGasShellPoison = 346,
        MortarShellExplosion = 347,
        MortarShellBody = 348,
        BomberBombExplosion = 349,
        BomberBombBody = 350,
        FourConsecutiveMissileBloodSmoke = 351,
        BloodGrenadeBlast = 352,
        BloodGrenadeBlastLv1Extend = 353,
        BloodGrenadeBlastLv2 = 354,
        BlastDangerBaggage = 355,
        MultiRodSwing = 356,
        MultiRodThrust = 357,
        MuleKick = 358,
        MulePush = 359,
        MuleTackle = 360,
        CliffPush = 361,
        CliffKick = 362,
        Ww1StrayBulletAssault = 363,
        Ww1StrayBulletMachinegun = 364,
        Ww2AirPlaneMachinegun = 365,
        HeavyPhysics = 366,
        TarWave = 367,
        ShakeFeet = 368,
        ToxicGasZone = 369,
        ElectricZone = 370,
        TranquilizerGasZone = 371,
        PlayerRopeCounter = 372,
        PlayerKill = 373,
        AmeliePush = 374,
        AimTarget = 375,
        MulePushGrass = 376,
        InvalidPhysicalAttackOnBT = 377,
        DragMuleFromVehicle = 378,
        RopeStumble = 379,
        ForceFallDown = 380,
        NpcDrown = 381,
        CliffCodeAttackSign = 382,
        CliffCodeAttack = 383,
        Ww2HeavyMachinegun = 384,
        InvalidPhysicalProjectileAttackOnBT = 385,
        CliffBarrier = 386,
        DemensElectricalGrenade = 387,
        __2 = 388,
    }

    [RTTI.Serializable(0x203B19E2B53B672C, GameType.DS)]
    public enum EDSAuralStimulusType : int8
    {
        GeneralNoise = 0,
        FootStep = 1,
        GunShot = 2,
        Ricochet = 3,
        Explosion = 4,
        Echo = 5,
        Voice = 6,
        Scream = 7,
        BloodGrenadeExplosion = 8,
        BaggageSensor = 9,
        BaggageSensorPassivePing = 10,
        VehicleEngine = 11,
        BaggageBounce = 12,
        GrenadeDrop = 13,
        PhysicsGimmickDrop = 14,
    }

    [RTTI.Serializable(0x99D7F1690E4AB33E, GameType.DS)]
    public enum EDSBBPodShaderFloat1Type : int8
    {
        GridMask = 0,
        Black = 1,
        LightIntensity = 2,
        BottomLightIntensity = 3,
        FadeRange = 4,
    }

    [RTTI.Serializable(0x655EAFD0340A5156, GameType.DS)]
    public enum EDSBBPodShaderFloat3Type : int8
    {
        LightColor = 0,
        BottomLightColor = 1,
    }

    [RTTI.Serializable(0xE6A68B94277D48C7, GameType.DS)]
    public enum EDSBackpackAccessoryType : int8
    {
        None = 0,
        FigureA = 1,
        FigureB = 2,
        FigureC = 3,
        FigureD = 4,
        FigureE = 5,
        FigureF = 6,
        FigureG = 7,
        FigureH = 8,
    }

    [RTTI.Serializable(0xDB0C639B6ACA29C0, GameType.DS)]
    public enum EDSBackpackOptionType : int8
    {
        None = 0,
        BatteryCharger = 1,
        UtilityPouch = 2,
        AmmoLoader = 3,
        FourGrenadePouch = 4,
        GrenadePouch = 5,
        Balancer = 6,
        BatteryChargerLv2 = 7,
        BatteryChargerLv3 = 8,
    }

    [RTTI.Serializable(0x7F4EDBF6223C3A5A, GameType.DS)]
    public enum EDSBaggage2CarrierType : int8
    {
        None = 0,
        Player = 1,
        Cart = 2,
        Truck = 3,
        Bike = 4,
        __0 = 5,
        Post = 6,
        Terminal = 7,
        Mule = 8,
        MulePost = 9,
        __1 = 10,
        __2 = 11,
        Ground = 12,
        AutoDeliveryCarrier = 13,
        TerminalShelf = 14,
        Warehouse = 15,
        MissionDeliveredStorage = 16,
        PrivateRoomStorage = 17,
        AreaChangeStorage = 18,
        SupportHunter = 19,
        PlayerBackup = 20,
    }

    [RTTI.Serializable(0xB86E34541915D30F, GameType.DS)]
    public enum EDSBaggage2Category : int8
    {
        Private = 0,
        SamMission = 1,
        SimpleMission = 2,
        OnlineMission = 3,
        Mule = 4,
        FriendlyMule = 5,
        Discarded = 6,
        DynamicOffline = 7,
        BossBattle = 8,
        BossBattle640 = 9,
        DynamicOffline640 = 10,
        Reward = 11,
        EnemyDeadbody = 12,
        OnlineTrash = 13,
        Cart = 14,
        DummyBaggage = 15,
        SpawnFromMule = 16,
        HalfLifeCube = 17,
    }

    [RTTI.Serializable(0x57DD848B3A6C989, GameType.DS)]
    public enum EDSBaggage2Model : int8
    {
        None = 0,
        NormalSmall = 1,
        NormalMiddle = 2,
        NormalLarge = 3,
        PersonalSmall = 4,
        PersonalMiddle = 5,
        PersonalLarge = 6,
        PostBuilder = 7,
        ZiplineBuilder = 8,
        BridgeBuilder = 9,
        MuleBaggage = 10,
        Cart = 11,
        OpenedCart = 12,
        Barrel = 13,
        Ladder = 14,
        WeaponCaseSmall = 15,
        WeaponCaseMiddle = 16,
        WeaponCaseLarge = 17,
        MaterialSmall = 18,
        MaterialMiddle = 19,
        MaterialLarge = 20,
        MaterialBarrel = 21,
        BombBarrel = 22,
        ArmShield = 23,
        WaistShield = 24,
        LeftShieldLv2 = 25,
        RightShieldLv2 = 26,
        SmallDelicate = 27,
        HeatParts = 28,
        HalfLifeCube = 29,
    }

    [RTTI.Serializable(0xC0C17F37119390AF, GameType.DS)]
    public enum EDSBaggage2Size : int8
    {
        Small = 0,
        Middle = 1,
        Large = 2,
        Human = 3,
        BB = 4,
        Shoes = 5,
    }

    [RTTI.Serializable(0xD02F88E35E79A92F, GameType.DS)]
    public enum EDSBaggage2SlotType : int8
    {
        Default = 0,
        BackPack = 1,
        LeftArm = 2,
        RightArm = 3,
        LeftWaist = 4,
        RightWaist = 5,
        LeftHand = 6,
        RightHand = 7,
        LeftCarrier = 8,
        RightCarrier = 9,
        MainWeapon = 10,
        SubWeapon = 11,
        SubWeaponPouch = 12,
        Item = 13,
        Shoes = 14,
        SpareShoes = 15,
        SuitParts = 16,
        Glove = 17,
        Mask = 18,
        Private = 19,
        Public = 20,
        PublicReceived = 21,
        Supply = 22,
        MuleBackPack = 24,
        AutoDeliveryCarrier1 = 25,
        AutoDeliveryCarrier2 = 26,
        TerminalShelfMission = 27,
        TerminalShelfSupply = 28,
        TerminalShelfOrder = 29,
        TerminalShelfDonation = 30,
        TerminalShelfBridgesDonation = 31,
        TerminalShelfReturn = 32,
        TerminalShelfProtection = 33,
        TerminalShelfCommit = 34,
        TerminalShelfProcurement = 35,
        TerminalShelfReward = 36,
        PassengerSeat = 69,
        TerminalShelfPublicCommit = 70,
        TerminalShelfPublicCommitMission = 71,
        AreaChangeSlot2 = 72,
        Trash = 73,
        __0 = 74,
    }

    [RTTI.Serializable(0x42B0021283AFFAA2, GameType.DS)]
    public enum EDSBaggage2Type : int8
    {
        Normal = 0,
        MainWeapon = 1,
        SubWeapon = 2,
        Item = 3,
        Cart = 4,
        Body = 5,
    }

    [RTTI.Serializable(0x61027C412C83740F, GameType.DS)]
    public enum EDSBaggageBrokenType : int8
    {
        None = 0,
        ByOther = 1,
        ByRiver = 2,
        ByEnemy = 3,
        ByFall = 4,
        ByAttack = 5,
        ByRain = 6,
    }

    [RTTI.Serializable(0x97CDB5503C56725A, GameType.DS)]
    public enum EDSBaggageDeliverdState : int8
    {
        None = 0,
        NotDelivered = 1,
        Delivered = 2,
        Broken = 3,
        Lost = 5,
        Online = 4,
    }

    [RTTI.Serializable(0x4764271FED919565, GameType.DS)]
    public enum EDSBaggageFuntionType : int32
    {
        None = 0,
        Post = 1,
    }

    [RTTI.Serializable(0xEDD3AFFDC146654A, GameType.DS)]
    public enum EDSBaggageMenuInfoResult : int32
    {
        MoveFromPlayer = 1,
        MoveToPlayer = 2,
        MoveFromGround = 4,
        MoveToGround = 8,
        MoveFromVehicle = 16,
        MoveToVehicle = 32,
        MoveFromBike = 64,
        MoveToBike = 128,
        MoveFromCart = 256,
        MoveToCart = 512,
        MoveFromPrivateBox = 1024,
        MoveToPrivateBox = 2048,
        MoveFromPublicBox = 4096,
        MoveToPublicBox = 8192,
        MoveFromSupplyBox = 16384,
        MoveToSupplyBox = 32768,
        MoveFromMaterialBox = 65536,
        MoveToMaterialBox = 131072,
        MoveToPlayerBackPack = 262144,
    }

    [RTTI.Serializable(0x395454B32EE51BF2, GameType.DS)]
    public enum EDSBareFootDamageType : int8
    {
        BareFootDamage_None = 0,
        BareFootDamage_Damaged = 1,
    }

    [RTTI.Serializable(0x1769C55DD5FE8418, GameType.DS)]
    public enum EDSBareFootNailDamageType : int8
    {
        BareFootDamage_NailExists = 0,
        BareFootDamage_NailDoesnotExist = 1,
    }

    [RTTI.Serializable(0x8ED2999FDD40A19C, GameType.DS)]
    public enum EDSBgmPhase : int8
    {
        None = 0,
        BtNotice = 1,
        BtDanger = 2,
        BtBattle = 3,
        BtDive = 4,
        BtLost = 5,
        MulePreCaution = 6,
        MuleCaution = 7,
        MuleNotice = 8,
        MuleReturn = 9,
        MuleBattle = 10,
        MuleBattleStolenBaggage = 11,
        MuleBattleNoChase = 12,
        MuleNoticeCantFind = 13,
        MuleVsBtNoBattle = 14,
        MuleVsBtBattle = 15,
        CliffNormal = 16,
        CliffCaution = 17,
        CliffEvation = 18,
        CliffNotice = 19,
        CliffAlert = 20,
        CliffEscape = 21,
        HiggsGun = 22,
        HiggsGun_Hidden = 23,
        HiggsKnife = 24,
        HiggsKnife_Hidden = 25,
        HiggsPunching_Fase1 = 26,
        HiggsPunching_Fase2 = 27,
    }

    [RTTI.Serializable(0xC113BFF6BCA27CB8, GameType.DS)]
    public enum EDSBossCliffMarkerType : int8
    {
        None = 0,
        Warrior01 = 1,
        Warrior02 = 2,
        Warrior03 = 3,
    }

    [RTTI.Serializable(0x425494950AB7075D, GameType.DS)]
    public enum EDSBottomlessSwampCollisionTriggerType : int8
    {
        None = 0,
    }

    [RTTI.Serializable(0x5EDC91625AC3A512, GameType.DS)]
    public enum EDSBridgeLengthType : int8
    {
        M30 = 0,
        M45 = 1,
    }

    [RTTI.Serializable(0xFE86B441C1E3BA7F, GameType.DS)]
    public enum EDSBulletLineType : int8
    {
        Default = 0,
        Middle = 1,
        Large = 2,
        Blood = 3,
        Gold = 4,
        Rubber = 5,
        Shotgun = 6,
        ShotgunBlood = 7,
        ShotgunGold = 8,
        ShotgunRubber = 9,
        NpcDefault = 10,
        NpcRubber = 11,
        NpcShotgun = 12,
        NpcGold = 13,
        NpcAirPlaneMachinegun = 14,
        NpcHeavyMachinegun = 15,
        NormalToBlood = 16,
        CapsuleBlood = 17,
        CapsuleBloodMiddle = 18,
        CapsuleBloodLarge = 19,
        GrenadeShell = 20,
        BloodTail = 21,
        ShotgunBloodTail = 22,
        CapsuleBloodTail = 23,
        CapsuleBloodTailMiddle = 24,
        CapsuleBloodTailLarge = 25,
        GoldTail = 26,
        ShotgunGoldTail = 27,
        NpcGoldTail = 28,
        Invalid = 29,
    }

    [RTTI.Serializable(0xB2950D403F0BF93B, GameType.DS)]
    public enum EDSCameraBaseType : int8
    {
        Normal = 0,
        Head = 1,
        HLP_Odradek = 2,
        Manual = 3,
        Helper = 4,
    }

    [RTTI.Serializable(0xB7B8FCF5DD9487C4, GameType.DS)]
    public enum EDSCameraCollisionTriggerType : int8
    {
        None = 0,
        NearTerminal = 1,
        UserType1 = 2,
        UserType2 = 3,
        UserType3 = 4,
        UserType4 = 5,
        UserType5 = 6,
        UserType6 = 7,
        UserType7 = 8,
        UserType8 = 9,
        UserType9 = 10,
        UserType10 = 11,
        UserType11 = 12,
        UserType12 = 13,
        UserType13 = 14,
        UserType14 = 15,
        UserType15 = 16,
        UserType16 = 17,
        AdditionalDistance = 18,
    }

    [RTTI.Serializable(0x1D438AE532DF0E4A, GameType.DS)]
    public enum EDSCameraParam : int16
    {
        Around_Default = 0,
        Around_Basic_Stand = 1,
        Around_Basic_Crouch = 2,
        Around_Basic_Dash = 3,
        Around_Basic_DashStart = 4,
        Around_Basic_Stand_Move = 5,
        Around_Basic_Crouch_Move = 6,
        Around_BasicAttackStand = 7,
        Around_BasicAttackCrouch = 8,
        Around_BasicAttackDash = 9,
        Around_BasicAttackDashStart = 10,
        Around_BasicAttackStandMove = 11,
        Around_BasicAttackCrouchMove = 12,
        Around_Carry_Stand = 13,
        Around_Carry_Crouch = 14,
        Around_Carry_Dash = 15,
        Around_Carry_DashStart = 16,
        Around_Carry_Stand_Move = 17,
        Around_Carry_Crouch_Move = 18,
        Around_CarryAttackStand = 19,
        Around_CarryAttackCrouch = 20,
        Around_CarryAttackDash = 21,
        Around_CarryAttackDashStart = 22,
        Around_CarryAttackStandMove = 23,
        Around_CarryAttackCrouchMove = 24,
        Around_Elude = 25,
        Around_StepOn = 26,
        Around_RiverDrive = 27,
        Around_FakeHolo = 28,
        Around_FakeHoloBike = 29,
        Around_HiggsTarryPhase = 30,
        Around_Odradek_DetectEnemy = 31,
        Around_Odradek_DetectEnemyInTruck = 32,
        Around_Odradek = 33,
        Around_Odradek_TurnToEnemy = 34,
        Around_Odradek_TurnToEnemyInTruck = 35,
        Around_HoodOn = 36,
        Around_HoodOff = 37,
        Around_ShoesBroken = 38,
        Around_SelectZipline = 39,
        Around_Sns = 40,
        Around_NearTerminal = 41,
        Around_DragCart = 42,
        Around_RideTruck = 43,
        Around_RideBike = 44,
        __0 = 45,
        Around_RideTruckOnRoad = 46,
        Around_RideBikeOnRoad = 47,
        Around_RideTruckBoost = 48,
        Around_RideBikeBoost = 49,
        Around_RideTruckBoostOnRoad = 50,
        Around_RideBikeBoostOnRoad = 51,
        Around_PrivateRoomDefault = 52,
        Around_CareForBBDefault = 53,
        Around_TakeABreakDefault = 54,
        Around_TakeABreakOnsen = 55,
        Around_TakeABreakHeartmanOnsen = 56,
        Around_TakeABreakBeach = 57,
        Around_RopeMove = 58,
        Around_RopeMove_Wall = 59,
        Around_MuleFaint = 60,
        Around_OdradekHappy = 61,
        Around_DefeatCatcher = 62,
        __1 = 63,
        Around_EquipWeapon = 64,
        __2 = 65,
        Around_Hacking = 66,
        Around_HackingCrouch = 67,
        Around_InteractSignboard = 68,
        Around_UserType1_Stand = 69,
        Around_UserType1_Crouch = 70,
        Around_UserType1_Dash = 71,
        Around_UserType1_DashStart = 72,
        Around_UserType1_StandMove = 73,
        Around_UserType1_CrouchMove = 74,
        Around_UserType1_AttackStand = 75,
        Around_UserType1_AttackCrouch = 76,
        Around_UserType1_AttackDash = 77,
        Around_UserType1_AttackDashStart = 78,
        Around_UserType1_AttackStandMove = 79,
        Around_UserType1_AttackCrouchMove = 80,
        Around_UserType2_Stand = 81,
        Around_UserType2_Crouch = 82,
        Around_UserType2_Dash = 83,
        Around_UserType2_DashStart = 84,
        Around_UserType2_StandMove = 85,
        Around_UserType2_CrouchMove = 86,
        Around_UserType2_AttackStand = 87,
        Around_UserType2_AttackCrouch = 88,
        Around_UserType2_AttackDash = 89,
        Around_UserType2_AttackDashStart = 90,
        Around_UserType2_AttackStandMove = 91,
        Around_UserType2_AttackCrouchMove = 92,
        Around_UserType3_Stand = 93,
        Around_UserType3_Crouch = 94,
        Around_UserType3_Dash = 95,
        Around_UserType3_DashStart = 96,
        Around_UserType3_StandMove = 97,
        Around_UserType3_CrouchMove = 98,
        Around_UserType3_AttackStand = 99,
        Around_UserType3_AttackCrouch = 100,
        Around_UserType3_AttackDash = 101,
        Around_UserType3_AttackDashStart = 102,
        Around_UserType3_AttackStandMove = 103,
        Around_UserType3_AttackCrouchMove = 104,
        Around_UserType4_Stand = 105,
        Around_UserType4_Crouch = 106,
        Around_UserType4_Dash = 107,
        Around_UserType4_DashStart = 108,
        Around_UserType4_StandMove = 109,
        Around_UserType4_CrouchMove = 110,
        Around_UserType4_AttackStand = 111,
        Around_UserType4_AttackCrouch = 112,
        Around_UserType4_AttackDash = 113,
        Around_UserType4_AttackDashStart = 114,
        Around_UserType4_AttackStandMove = 115,
        Around_UserType4_AttackCrouchMove = 116,
        Around_UserType5_Stand = 117,
        Around_UserType5_Crouch = 118,
        Around_UserType5_Dash = 119,
        Around_UserType5_DashStart = 120,
        Around_UserType5_StandMove = 121,
        Around_UserType5_CrouchMove = 122,
        Around_UserType5_AttackStand = 123,
        Around_UserType5_AttackCrouch = 124,
        Around_UserType5_AttackDash = 125,
        Around_UserType5_AttackDashStart = 126,
        Around_UserType5_AttackStandMove = 127,
        Around_UserType5_AttackCrouchMove = 128,
        Around_UserType6_Stand = 129,
        Around_UserType6_Crouch = 130,
        Around_UserType6_Dash = 131,
        Around_UserType6_DashStart = 132,
        Around_UserType6_StandMove = 133,
        Around_UserType6_CrouchMove = 134,
        Around_UserType6_AttackStand = 135,
        Around_UserType6_AttackCrouch = 136,
        Around_UserType6_AttackDash = 137,
        Around_UserType6_AttackDashStart = 138,
        Around_UserType6_AttackStandMove = 139,
        Around_UserType6_AttackCrouchMove = 140,
        Around_UserType7_Stand = 141,
        Around_UserType7_Crouch = 142,
        Around_UserType7_Dash = 143,
        Around_UserType7_DashStart = 144,
        Around_UserType7_StandMove = 145,
        Around_UserType7_CrouchMove = 146,
        Around_UserType7_AttackStand = 147,
        Around_UserType7_AttackCrouch = 148,
        Around_UserType7_AttackDash = 149,
        Around_UserType7_AttackDashStart = 150,
        Around_UserType7_AttackStandMove = 151,
        Around_UserType7_AttackCrouchMove = 152,
        Around_UserType8_Stand = 153,
        Around_UserType8_Crouch = 154,
        Around_UserType8_Dash = 155,
        Around_UserType8_DashStart = 156,
        Around_UserType8_StandMove = 157,
        Around_UserType8_CrouchMove = 158,
        Around_UserType8_AttackStand = 159,
        Around_UserType8_AttackCrouch = 160,
        Around_UserType8_AttackDash = 161,
        Around_UserType8_AttackDashStart = 162,
        Around_UserType8_AttackStandMove = 163,
        Around_UserType8_AttackCrouchMove = 164,
        Around_UserType9_Stand = 165,
        Around_UserType9_Crouch = 166,
        Around_UserType9_Dash = 167,
        Around_UserType9_DashStart = 168,
        Around_UserType9_StandMove = 169,
        Around_UserType9_CrouchMove = 170,
        Around_UserType9_AttackStand = 171,
        Around_UserType9_AttackCrouch = 172,
        Around_UserType9_AttackDash = 173,
        Around_UserType9_AttackDashStart = 174,
        Around_UserType9_AttackStandMove = 175,
        Around_UserType9_AttackCrouchMove = 176,
        Around_UserType10_Stand = 177,
        Around_UserType10_Crouch = 178,
        Around_UserType10_Dash = 179,
        Around_UserType10_DashStart = 180,
        Around_UserType10_StandMove = 181,
        Around_UserType10_CrouchMove = 182,
        Around_UserType10_AttackStand = 183,
        Around_UserType10_AttackCrouch = 184,
        Around_UserType10_AttackDash = 185,
        Around_UserType10_AttackDashStart = 186,
        Around_UserType10_AttackStandMove = 187,
        Around_UserType10_AttackCrouchMove = 188,
        Around_UserType11_Stand = 189,
        Around_UserType11_Crouch = 190,
        Around_UserType11_Dash = 191,
        Around_UserType11_DashStart = 192,
        Around_UserType11_StandMove = 193,
        Around_UserType11_CrouchMove = 194,
        Around_UserType11_AttackStand = 195,
        Around_UserType11_AttackCrouch = 196,
        Around_UserType11_AttackDash = 197,
        Around_UserType11_AttackDashStart = 198,
        Around_UserType11_AttackStandMove = 199,
        Around_UserType11_AttackCrouchMove = 200,
        Around_UserType12_Stand = 201,
        Around_UserType12_Crouch = 202,
        Around_UserType12_Dash = 203,
        Around_UserType12_DashStart = 204,
        Around_UserType12_StandMove = 205,
        Around_UserType12_CrouchMove = 206,
        Around_UserType12_AttackStand = 207,
        Around_UserType12_AttackCrouch = 208,
        Around_UserType12_AttackDash = 209,
        Around_UserType12_AttackDashStart = 210,
        Around_UserType12_AttackStandMove = 211,
        Around_UserType12_AttackCrouchMove = 212,
        Around_UserType13_Stand = 213,
        Around_UserType13_Crouch = 214,
        Around_UserType13_Dash = 215,
        Around_UserType13_DashStart = 216,
        Around_UserType13_StandMove = 217,
        Around_UserType13_CrouchMove = 218,
        Around_UserType13_AttackStand = 219,
        Around_UserType13_AttackCrouch = 220,
        Around_UserType13_AttackDash = 221,
        Around_UserType13_AttackDashStart = 222,
        Around_UserType13_AttackStandMove = 223,
        Around_UserType13_AttackCrouchMove = 224,
        Around_UserType14_Stand = 225,
        Around_UserType14_Crouch = 226,
        Around_UserType14_Dash = 227,
        Around_UserType14_DashStart = 228,
        Around_UserType14_StandMove = 229,
        Around_UserType14_CrouchMove = 230,
        Around_UserType14_AttackStand = 231,
        Around_UserType14_AttackCrouch = 232,
        Around_UserType14_AttackDash = 233,
        Around_UserType14_AttackDashStart = 234,
        Around_UserType14_AttackStandMove = 235,
        Around_UserType14_AttackCrouchMove = 236,
        Around_UserType15_Stand = 237,
        Around_UserType15_Crouch = 238,
        Around_UserType15_Dash = 239,
        Around_UserType15_DashStart = 240,
        Around_UserType15_StandMove = 241,
        Around_UserType15_CrouchMove = 242,
        Around_UserType15_AttackStand = 243,
        Around_UserType15_AttackCrouch = 244,
        Around_UserType15_AttackDash = 245,
        Around_UserType15_AttackDashStart = 246,
        Around_UserType15_AttackStandMove = 247,
        Around_UserType15_AttackCrouchMove = 248,
        Around_UserType16_Stand = 249,
        Around_UserType16_Crouch = 250,
        Around_UserType16_Dash = 251,
        Around_UserType16_DashStart = 252,
        Around_UserType16_StandMove = 253,
        Around_UserType16_CrouchMove = 254,
        Around_UserType16_AttackStand = 255,
        Around_UserType16_AttackCrouch = 256,
        Around_UserType16_AttackDash = 257,
        Around_UserType16_AttackDashStart = 258,
        Around_UserType16_AttackStandMove = 259,
        Around_UserType16_AttackCrouchMove = 260,
        Tps_Default = 261,
        Tps_Basic_Stand = 262,
        Tps_Basic_Dash = 263,
        Tps_Basic_Crouch = 264,
        Tps_Builder_Hold = 265,
        Tps_SprayToSelf = 266,
        __3 = 267,
        Tps_Watch_Tower_Builder_Hold = 268,
        Tps_Bridge_Builder_Hold = 269,
        Tps_Safety_House_Builder_Hold = 270,
        Tps_RopeHold = 271,
        Tps_RopePile = 272,
        Tps_BolaStand = 273,
        Tps_BolaDash = 274,
        Tps_BolaCrouch = 275,
        Tps_LadderHold = 276,
        Tps_CamouflageHold = 277,
        Tps_CamouflageUsing = 278,
        Tps_MissileStand = 279,
        Tps_MissileDash = 280,
        Tps_MissileCrouch = 281,
        Tps_Pee = 282,
        Tps_Strand = 283,
        Tps_Sticky = 284,
        Tps_ProjectileAim = 285,
        Tps_ProjectileAimCharge = 286,
        Subjective_Default = 287,
        Subjective_Bike = 288,
        Subjective_Truck = 289,
        Subjective_WatchTower = 290,
        Subjective_ShowBB = 291,
        Subjective_DraggedByGazer = 292,
        Subjective_ChangeToWatchTower = 293,
        Subjective_InsideTruck = 294,
        Subjective_HeartmanRoom = 295,
        KnotSpace_Default = 296,
        KnotSpace_Ghost = 297,
        CutIn_Default = 298,
        Subspace_Default = 299,
        Subspace_Truck = 300,
        Subspace_Bike = 301,
        Subspace_Shelf = 302,
        Subspace_Cart = 303,
        Subspace_Pouch = 304,
        Subspace_CustomizeBackpack = 305,
        Subspace_CustomizeConstruction = 306,
        Subspace_CustomizeConstructionNear = 307,
        Subspace_CustomizeConstructionFar = 308,
        Subspace_CustomizeConstructionSuperLearge = 309,
        Subspace_CustomizeConstructionSpecial1 = 310,
        Subspace_CustomizeConstructionSpecial2 = 311,
        Subspace_Backpack = 312,
        Subspace_LeftShoulder = 313,
        Subspace_RightShoulder = 314,
        Subspace_LeftWaist = 315,
        Subspace_RightWaist = 316,
        Subspace_UtilityPouch = 317,
        Subspace_BackpackOption = 318,
        Subspace_Head = 319,
        Subspace_Hand = 320,
        Subspace_Skeleton = 321,
        Subspace_Foot = 322,
        Subspace_ToolHanger = 323,
        Subspace_ShoesHanger = 324,
        Subspace_Glove = 325,
        Subspace_Field = 326,
        Subspace_VehicleSelectTruck = 327,
        Subspace_VehicleSelectBike = 328,
        __4 = 329,
        Subspace_Bridge = 330,
        Subspace_WatchTower = 331,
        Subspace_SafetyHouse = 332,
        Subspace_ConstructionNormal = 333,
        Subspace_ConstructionSmall = 334,
        Subspace_ConstructionLearge = 335,
        Subspace_UniformCustomizeDefault = 336,
        Subspace_UniformCustomizeGlasses = 337,
        Subspace_UniformCustomizeCap = 338,
        Subspace_CustomizeBackpackAccessory = 339,
        PrivateRoom_Default = 340,
        Showdown_Default = 341,
        Zipline_Select = 342,
        Zipline_Hold = 343,
        Zipline_Move = 344,
        Zipline_RideOn = 345,
        Zipline_RideOff = 346,
        PhotoMode_Default = 347,
        Subjective_Zipline = 348,
        Tps_ZiplineHold = 349,
        Around_ZiplineMove = 350,
        Around_HoodOnOff = 351,
    }

    [RTTI.Serializable(0x549AC848C5E58232, GameType.DS)]
    public enum EDSCameraStockType : int8
    {
        EnableSwitch = 0,
        RightFixed = 1,
        LeftFixed = 2,
        DisableSwitch = 3,
    }

    [RTTI.Serializable(0xDCA5085B50B621E9, GameType.DS)]
    public enum EDSCameraType : int8
    {
        Around = 0,
        Tps = 1,
        Subjective = 2,
        KnotSpace = 3,
        CutIn = 4,
        Subspace = 5,
        PrivateRoom = 6,
        Showdown = 7,
        Zipline = 8,
        PhotoMode = 9,
        Unknown = 10,
    }

    [RTTI.Serializable(0x75EEC53A1EA7020, GameType.DS)]
    public enum EDSCarriableSize : int32
    {
        SizeS = 0,
        SizeM = 1,
        SizeL = 2,
    }

    [RTTI.Serializable(0x4E8049D6AB070124, GameType.DS)]
    public enum EDSCarriableType : int32
    {
        Baggage = 0,
        Humanoid = 1,
        Transporter = 2,
        Item = 3,
        BB = 4,
    }

    [RTTI.Serializable(0xCBF6ABDDDB4CC91F, GameType.DS)]
    public enum EDSCatalogueListItem_SpecialItemId : int32
    {
        PostConstructionMachine = 1073741824,
        BridgeConstructionMachine = 1073741825,
        ZiplineConstructionMachine = 1073741826,
        Cart = 1073741840,
        Motorcycle = 1073741841,
        Truck = 1073741842,
        MuleBaggage = 1073741872,
    }

    [RTTI.Serializable(0xA2ACAC3AE08A657E, GameType.DS)]
    public enum EDSCatcherControlCommand : int8
    {
        BringCatcher = 0,
        ForceOrderToReturn = 1,
        PrepareBringCatcher = 2,
        AllowCreateCrater = 3,
        ForceDeactivate = 4,
    }

    [RTTI.Serializable(0x5378DC45F5286223, GameType.DS)]
    public enum EDSCatcherJellyfishInitalState : int8
    {
        Floating = 0,
        Fixed = 1,
        Homing = 2,
        Divided = 3,
    }

    [RTTI.Serializable(0xB49EF2437F3F76A6, GameType.DS)]
    public enum EDSCatcherManagerNotifyEventType : int8
    {
        StartStrandObjec = 0,
        VisibleCatcher = 1,
        AllowCreateCrater = 2,
    }

    [RTTI.Serializable(0xFC0A413A7DBE6FE4, GameType.DS)]
    public enum EDSCatcherNotifyFlags : int8
    {
        IsBossType = 0,
        HasReceivedBloodGrenade = 1,
    }

    [RTTI.Serializable(0x3B08AFC47762BEE8, GameType.DS)]
    public enum EDSCatcherNotifyType : int8
    {
        BringCatcher = 0,
        KillCatcher = 1,
        AbandonCombat = 2,
        Annihilating = 3,
    }

    [RTTI.Serializable(0xFD0E7D3DE42EB611, GameType.DS)]
    public enum EDSCatcherStateType : int8
    {
        Dummy = 0,
        ActiveCatcherCount = 1,
        IsInCombat = 2,
    }

    [RTTI.Serializable(0x4E25366B00C3A65A, GameType.DS)]
    public enum EDSCatcherTarType : int8
    {
        Normal = 0,
        TarBelt = 1,
    }

    [RTTI.Serializable(0x2A5B198F3659DBC9, GameType.DS)]
    public enum EDSCatcherTitanHiggsLocation : int8
    {
        Stomach = 0,
        Shoulder = 1,
        Flank = 2,
        Chest = 3,
    }

    [RTTI.Serializable(0x1FD9B4289ED4DDF, GameType.DS)]
    public enum EDSCatcherType : int8
    {
        Octpus = 0,
        Quad = 1,
        Whale = 2,
        Titan = 3,
        Ghost = 4,
        Reserve5 = 5,
        Reserve6 = 6,
        Reserve7 = 7,
        Max = 8,
    }

    [RTTI.Serializable(0x5FB0555FA27F1849, GameType.DS)]
    public enum EDSCatcherWhaleRequestActionType : int8
    {
        ShowAttack = 0,
    }

    [RTTI.Serializable(0x5AE573932D9D6DDD, GameType.DS)]
    public enum EDSCheckStationScanInfo : int32
    {
        NoProblem = 0,
        BreakRequest = 1,
        ShoeseRequest = 2,
        VehicleRequest = 3,
    }

    [RTTI.Serializable(0xE7A024CA0CD42413, GameType.DS)]
    public enum EDSCheckStationState : int8
    {
        Idle = 0,
        Accept = 2,
        Reject = 3,
        Invisible = 5,
        Emergency = 4,
        Invalid = 6,
    }

    [RTTI.Serializable(0x6BC417AF7784D181, GameType.DS)]
    public enum EDSCollectibleType : int8
    {
        Crystal_S = 0,
        Crystal_M = 1,
        Crystal_L = 2,
        ShoeSoleGrass = 3,
        Cryptobiosis = 4,
        CryptobiosisClusterS = 5,
        CryptobiosisClusterM = 6,
        CryptobiosisClusterL = 7,
        CryptobiosisNotEscapeS = 8,
        CryptobiosisNotEscapeM = 9,
        CryptobiosisNotEscape = 10,
        CryptobiosisNotEscapeWithoutCoral = 11,
        CryptobiosisMushroomS = 12,
        CryptobiosisMushroomM = 13,
        Crystal_S_NotRandom = 14,
        Crystal_M_NotRandom = 15,
        Crystal_L_NotRandom = 16,
        Crystal_Drop_S = 17,
        Crystal_Drop_M = 18,
        Crystal_Drop_L = 19,
        CollectorsItem_NotRandom = 20,
        ShoeSoleGrassByLocator2 = 21,
        ShoeSoleGrassByLocator3 = 22,
    }

    [RTTI.Serializable(0x310A3B3684CDE9E8, GameType.DS)]
    public enum EDSCollectorsItemType : int8
    {
        Picked = 1,
        Reward_Sound = 2,
        Reward_Holo = 4,
        Reward_Voice = 8,
        Reward_Special_Sound = 18,
        Reward_Special_Holo = 20,
        Reward_Special_Voice = 24,
    }

    [RTTI.Serializable(0x563963F96F38D2E9, GameType.DS)]
    public enum EDSConstructionCollisionCheckShape : int32
    {
        Cylinder = 0,
        OrientedBox = 1,
    }

    [RTTI.Serializable(0x42FB26AEFB8AD4B, GameType.DS)]
    public enum EDSConstructionMenuInfoResult : int32
    {
        ChargeMaterial = 1,
        ChargeChiral = 4,
        LevelUp = 2,
    }

    [RTTI.Serializable(0xF25C7758F881E482, GameType.DS)]
    public enum EDSConstructionPointCategory : int8
    {
        DeliveryBase = 0,
        PreppersShelter = 2,
        SafetyHouse = 1,
        Post = 3,
        WatchTower = 4,
        __0 = 5,
        Charger = 6,
        RainShelter = 7,
        MulePost = 8,
        Zipline = 9,
        Ladder = 10,
        FieldRope = 11,
        Bridge30m = 13,
        Bridge45m = 12,
        RoadRebuilder = 14,
        __1 = 15,
        Invalid = 16,
    }

    [RTTI.Serializable(0x3EECCC483F37A663, GameType.DS)]
    public enum EDSConstructionPointNetType : int8
    {
        Stage = 0,
        Player = 1,
        Net = 2,
    }

    [RTTI.Serializable(0x270663132034C81B, GameType.DS)]
    public enum EDSConstructionPointSlotType : int8
    {
        Private = 0,
        Public = 1,
        Suplly = 2,
        __0 = 4,
    }

    [RTTI.Serializable(0xC5380B4B5540725D, GameType.DS)]
    public enum EDSConstructionPointState : int8
    {
        Inactive = 0,
        Unrealized = 1,
        Realized = 2,
        Removing = 3,
    }

    [RTTI.Serializable(0x5A786C45057DA643, GameType.DS)]
    public enum EDSConstructionPointType : int8
    {
        DeliveryBase = 0,
        PreppersShelter = 1,
        StageSafetyHouse = 2,
        PlayerSafetyHouse = 3,
        NetSafetyHouse = 4,
        StagePost = 5,
        PlayerPost = 6,
        NetPost = 7,
        StageWatchTower = 8,
        PlayerWatchTower = 9,
        NetWatchTower = 10,
        __0 = 11,
        __1 = 12,
        __2 = 13,
        StageCharger = 14,
        PlayerCharger = 15,
        NetCharger = 16,
        StageRainShelter = 17,
        PlayerRainShelter = 18,
        NetRainShelter = 19,
        MulePost = 20,
        StageZipline = 21,
        PlayerZipline = 22,
        NetZipline = 23,
        StageLadder = 24,
        PlayerLadder = 25,
        NetLadder = 26,
        StageFieldRope = 27,
        PlayerFieldRope = 28,
        NetFieldRope = 29,
        StageBridge30m = 30,
        PlayerBridge30m = 31,
        NetBridge30m = 32,
        StageBridge45m = 33,
        PlayerBridge45m = 34,
        NetBridge45m = 35,
        RoadRebuilder = 36,
        __3 = 37,
        StageBridge = 33,
        PlayerBridge = 34,
        NetBridge = 35,
    }

    [RTTI.Serializable(0x1FA0843A674E794, GameType.DS)]
    public enum EDSContactableType : int8
    {
        None = 0,
        Physical = 1,
        Bt = 2,
        All = 3,
    }

    [RTTI.Serializable(0x63DF13ECFA41F14E, GameType.DS)]
    public enum EDSCountAttrFlag : int32
    {
        None = 0,
        Save = 2,
    }

    [RTTI.Serializable(0x4F50A99653B3530C, GameType.DS)]
    public enum EDSCountTimeType : int8
    {
        SceneRunnningTime = 0,
        UserPlay_MissionTime = 1,
        UserPlay_InGameTime = 2,
        MissionEventTime = 3,
        MissionEventTime_StopPrivateRoom = 4,
        MissionEventTime_UserAwayStop = 5,
    }

    [RTTI.Serializable(0x5F864B9F79BF93FE, GameType.DS)]
    public enum EDSCountTimerState : int32
    {
        STATE_NONE = -1,
        STATE_COUNT_RUN = 0,
        STATE_COUNT_PAUSE = 1,
        STATE_COUNT_STANBY = 2,
        STATE_COUNT_END = 3,
    }

    [RTTI.Serializable(0xF5075786AB9697C, GameType.DS)]
    public enum EDSCoverAndAnimModelPartType : int8
    {
        Facial = 0,
        Hair = 1,
        Cloth = 2,
        Cloth_B = 3,
        Cloth_C = 4,
        Cloth_D = 5,
        LeftLegFinger = 6,
        RightLegFinger = 7,
        Dummy = 8,
    }

    [RTTI.Serializable(0x17DEA9E0634D83D8, GameType.DS)]
    public enum EDSCustomSoundMessageEventType : int8
    {
        PlayerFootStepRoot = 0,
        PlayerFootStepLeft = 1,
        PlayerFootStepRight = 2,
        PlayerHandLTouch = 3,
        PlayerHandRTouch = 4,
        PlayerStepOnPoint = 5,
        PlayerChestFront = 6,
    }

    [RTTI.Serializable(0xDCD58566C08FCF95, GameType.DS)]
    public enum EDSCutsceneStateAttr : int32
    {
        None = 0,
        HUDHideImmediately = 1,
        HUDHideFadeout = 2,
    }

    [RTTI.Serializable(0xFF8D1BF7F36537E9, GameType.DS)]
    public enum EDSDBBVoicePriority : int32
    {
        Death = 6,
        Damage = 5,
        LowDamage = 4,
        LowLowDamage = 3,
        Speech = 2,
        HighBreath = 1,
        UsualBreath = 0,
    }

    [RTTI.Serializable(0x4F2102AE0C75F438, GameType.DS)]
    public enum EDSDeliveryManagerCommandTarget : int8
    {
        Baggage = 0,
        DeliveryPoint = 1,
        Zipline = 2,
        Ladder = 3,
        FieldRope = 4,
        Bridge = 5,
    }

    [RTTI.Serializable(0x7DAFE8F2E7769C5B, GameType.DS)]
    public enum EDSDetonatableCondition : int8
    {
        None = 0,
        Always = 1,
        AfterContact = 2,
    }

    [RTTI.Serializable(0x564A66A03E77021D, GameType.DS)]
    public enum EDSDifficulty : int8
    {
        VeryEasy = 0,
        Easy = 1,
        Normal = 2,
        Hard = 3,
        VeryHard = 4,
    }

    [RTTI.Serializable(0xC22840FEC98CB2B6, GameType.DS)]
    public enum EDSEffectShapeForMissionMarkerType : int8
    {
        Auto = 0,
        None = 1,
        Cylinder = 2,
        Circle = 3,
    }

    [RTTI.Serializable(0x793EA1A60DF73520, GameType.DS)]
    public enum EDSEvaluationActingType : int32
    {
        None = 0,
        Bad = 1,
        Normal = 2,
        Good = 3,
    }

    [RTTI.Serializable(0xC8D1DCB20023D5C2, GameType.DS)]
    public enum EDSEvaluationType : int32
    {
        Unset = -1,
        Speed = 0,
        Safety = 1,
        BaggageCount = 3,
        BaggageWeight = 4,
        Service = 2,
        ServiceSpeed = 5,
        ServiceSafety = 6,
        ServiceBaggageCount = 7,
        ServiceBaggageWeight = 8,
        ServiceBoss = 9,
        ServiceEscape = 10,
        BridgeLink = 12,
    }

    [RTTI.Serializable(0x49F990CDEF749F7F, GameType.DS)]
    public enum EDSFunction : int32
    {
        kDSFunction_None = -1,
        DS_PAD_HOLD = 0,
        DS_PAD_SHOOT = 1,
        DS_PAD_CHARGE_SHOOT = 2,
        DS_PAD_GHOST_CAMERA_UP = 3,
        DS_PAD_GHOST_CAMERA_DOWN = 4,
        DS_PAD_GHOST_CAMERA_ROLL_RIGHT = 5,
        DS_PAD_GHOST_CAMERA_ROLL_LEFT = 6,
        DS_PAD_STOCK_CHANGE = 7,
        DS_PAD_ZOOMCAMERA = 7,
        DS_PAD_DASH = 8,
        DS_PAD_SUBJECT = 9,
        DS_PAD_ACTION = 10,
        DS_PAD_RELOAD = 11,
        DS_PAD_DODGE = 12,
        DS_PAD_STANCE = 13,
        DS_PAD_PICKUP = 14,
        DS_PAD_PICKUP_ONEHAND_R = 15,
        DS_PAD_PICKUP_ONEHAND_L = 16,
        DS_PAD_USE_SUITPARTS = 17,
        DS_PAD_MENU = 18,
        DS_PAD_OPTIONS = 19,
        DS_PAD_UP = 20,
        DS_PAD_DOWN = 21,
        DS_PAD_LEFT = 22,
        DS_PAD_RIGHT = 23,
        DS_UIPAD_SELECT_PRIMARY = 24,
        DS_UIPAD_SELECT_SUB = 25,
        DS_UIPAD_SELECT_ITEM = 26,
        DS_UIPAD_SELECT_EQUIP = 27,
        DS_UIPAD_RIGHT = 28,
        DS_UIPAD_LEFT = 29,
        DS_UIPAD_UP = 30,
        DS_UIPAD_DOWN = 31,
        DS_UIPAD_UPDOWN = 32,
        DS_UIPAD_LEFTRIGHT = 33,
        DS_UIPAD_UP_HOLD = 34,
        DS_UIPAD_DOWN_HOLD = 35,
        DS_UIPAD_LEFT_HOLD = 36,
        DS_UIPAD_RIGHT_HOLD = 37,
        DS_UIPAD_OK = 38,
        DS_UIPAD_CANCEL = 39,
        DS_UIPAD_OPTION = 40,
        DS_UIPAD_DETAIL = 41,
        DS_UIPAD_SELECT_BACK = 42,
        DS_UIPAD_SELECT_NEXT = 43,
        DS_UIPAD_ZOOM_OUT = 44,
        DS_UIPAD_ZOOM_IN = 45,
        DS_UIPAD_OPEN_MENU = 46,
        DS_UIPAD_PAUSE = 47,
        DS_UIPAD_L1 = 48,
        DS_UIPAD_R1 = 49,
        DS_UIPAD_L2 = 50,
        DS_UIPAD_R2 = 51,
        DS_UIPAD_L3 = 52,
        DS_UIPAD_R3 = 53,
        DS_UIPAD_L_STICK = 54,
        DS_UIPAD_L_STICK_LEFT = 55,
        DS_UIPAD_L_STICK_RIGHT = 56,
        DS_UIPAD_L_STICK_UP = 57,
        DS_UIPAD_L_STICK_DOWN = 58,
        DS_UIPAD_R_STICK = 59,
        DS_UIPAD_R_STICK_LEFT = 60,
        DS_UIPAD_R_STICK_RIGHT = 61,
        DS_UIPAD_R_STICK_UP = 62,
        DS_UIPAD_R_STICK_DOWN = 63,
        DS_UIPAD_L2R2 = 64,
        DS_UIPAD_FIGHT = 65,
        DS_UIPAD_SHAKE = 66,
        DS_UIPAD_L_STICK_ROTATE = 67,
        DS_PAD_TRIGGER_LEFT = 68,
        DS_PAD_TRIGGER_RIGHT = 69,
        DS_PAD_L_STICK_UP = 70,
        DS_PAD_L_STICK_DOWN = 71,
        DS_PAD_L_STICK_LEFT = 72,
        DS_PAD_L_STICK_RIGHT = 73,
        DS_PAD_R_STICK_UP = 74,
        DS_PAD_R_STICK_DOWN = 75,
        DS_PAD_R_STICK_LEFT = 76,
        DS_PAD_R_STICK_RIGHT = 77,
        EDSPadGameButtonType_STANCE = 78,
        EDSPadGameButtonType_DODGE = 79,
        EDSPadGameButtonType_SUB_ACTION_LONG = 80,
        EDSPadGameButtonType_ACTION = 81,
        EDSPadGameButtonType_ACCESS = 82,
        EDSPadGameButtonType_MELEE = 83,
        EDSPadGameButtonType_RIDE_GETOFF = 84,
        EDSPadGameButtonType_SUBJECTIVE = 85,
        EDSPadGameButtonType_FIRE = 86,
        EDSPadGameButtonType_STOCK = 87,
        EDSPadGameButtonType_PICKUP = 88,
        EDSPadGameButtonType_HOLD = 89,
        EDSPadGameButtonType_DASH = 90,
        EDSPadGameButtonType_PICKUP_R = 91,
        EDSPadGameButtonType_PICKUP_L = 92,
        EDSPadGameButtonType_USE_SUITPARTS = 93,
        EDSPadGameButtonType_USE_BAGGAGE_SENSOR = 94,
        EDSPadGameButtonType_SELECT = 95,
        EDSPadGameButtonType_OPTIONS = 96,
        EDSPadGameButtonType_BACK = 97,
        EDSPadGameButtonType_PAD_LEFT = 98,
        EDSPadGameButtonType_PAD_RIGHT = 99,
        EDSPadGameButtonType_PAD_UP = 100,
        EDSPadGameButtonType_PAD_DOWN = 101,
        EDSPadGameButtonType_BREATH_STOP = 102,
        EDSPadGameButtonType_ZOOM = 103,
        EDSPadGameButtonType_FIGHT = 104,
        EDSPadGameButtonType_SELECT_MAIN_WEAPON = 105,
        EDSPadGameButtonType_SELECT_MAGAZINE = 106,
        EDSPadGameButtonType_SELECT_SUIT_PARTS = 107,
        EDSPadGameButtonType_SELECT_ITEM = 108,
        EDSPadGameButtonType_SELECT_EQUIPMENT = 109,
        EDSPadGameButtonType_CHECK = 110,
        EDSPadGameButtonType_DETAIL = 111,
        EDSPadGameButtonType_TAB_BACK = 112,
        EDSPadGameButtonType_TAB_NEXT = 113,
        EDSPadGameButtonType_OK = 114,
        EDSPadGameButtonType_CANCEL = 115,
        EDSPadGameButtonType_DPAD_LEFT = 116,
        EDSPadGameButtonType_DPAD_RIGHT = 117,
        EDSPadGameButtonType_DPAD_UP = 118,
        EDSPadGameButtonType_DPAD_DOWN = 119,
        EDSPadGameButtonType_L1 = 120,
        EDSPadGameButtonType_L2 = 121,
        EDSPadGameButtonType_L3 = 122,
        EDSPadGameButtonType_R1 = 123,
        EDSPadGameButtonType_R2 = 124,
        EDSPadGameButtonType_R3 = 125,
        EDSPadGameButtonType_PRIVATE_ROOM_BUTTON_0 = 126,
        EDSPadGameButtonType_PRIVATE_ROOM_BUTTON_1 = 127,
        EDSPadGameButtonType_PRIVATE_ROOM_BUTTON_2 = 128,
        EDSPadGameButtonType_PRIVATE_ROOM_BUTTON_3 = 129,
        EDSPadGameButtonType_COMMON_MARKER = 130,
        EDSPadGameButtonType_COMMON_DECIDE = 131,
        EDSPadGameButtonType_COMMON_CANCEL = 132,
        EDSPadGameButtonType_COMPASS_FOCUS_LR = 133,
        EDSPadGameButtonType_L_STICK = 134,
        EDSPadGameButtonType_R_STICK = 135,
        EDSPadGameButtonType_L_STICK_LEFT = 136,
        EDSPadGameButtonType_L_STICK_RIGHT = 137,
        EDSPadGameButtonType_L_STICK_UP = 138,
        EDSPadGameButtonType_L_STICK_DOWN = 139,
        EDSPadGameButtonType_R_STICK_LEFT = 140,
        EDSPadGameButtonType_R_STICK_RIGHT = 141,
        EDSPadGameButtonType_R_STICK_UP = 142,
        EDSPadGameButtonType_R_STICK_DOWN = 143,
        EDSPadGameButtonType_R_STICK_ALL_DIR = 144,
        EDSPadGameButtonType_UP_DOWN = 145,
        EDSPadGameButtonType_SHAKE = 146,
        EDSPadGameButtonType_SHAKE_BY_KEYBOARD = 147,
        EDSPadGameButtonType_L_STICK_ROTATE = 148,
        EDSPadGameButtonType_R_STICK_ROTATE = 149,
        EMenuInputFunction_FUNCTION_MOUSE_ACCEPT = 150,
        EMenuInputFunction_FUNCTION_MOUSE_CANCEL = 151,
        EMenuInputFunction_FUNCTION_MOUSE_MIDDLE = 152,
        EMenuInputFunction_FUNCTION_MOUSE_OVER = 153,
        EMenuInputFunction_FUNCTION_MOUSE_RANGE_OUT = 154,
        EMenuInputFunction_FUNCTION_MOUSE_SCROLL_UP = 155,
        EMenuInputFunction_FUNCTION_MOUSE_SCROLL_DOWN = 156,
        EMenuInputFunction_FUNCTION_MOUSE_ACCEPT_PAD = 157,
        EMenuInputFunction_FUNCTION_MOUSE_SCROLL_UP_PAD = 158,
        EMenuInputFunction_FUNCTION_MOUSE_SCROLL_DOWN_PAD = 159,
        EMenuInputFunction_FUNCTION_DPAD_NAV_UP = 160,
        EMenuInputFunction_FUNCTION_DPAD_NAV_DOWN = 161,
        EMenuInputFunction_FUNCTION_DPAD_NAV_LEFT = 162,
        EMenuInputFunction_FUNCTION_DPAD_NAV_RIGHT = 163,
        EMenuInputFunction_FUNCTION_NAV_UP = 164,
        EMenuInputFunction_FUNCTION_NAV_DOWN = 165,
        EMenuInputFunction_FUNCTION_NAV_LEFT = 166,
        EMenuInputFunction_FUNCTION_NAV_RIGHT = 167,
        EMenuInputFunction_FUNCTION_SCROLL_UP = 168,
        EMenuInputFunction_FUNCTION_SCROLL_DOWN = 169,
        EMenuInputFunction_FUNCTION_ACCEPT = 170,
        EMenuInputFunction_FUNCTION_OPEN_VKB = 171,
        EMenuInputFunction_FUNCTION_CANCEL = 172,
        EMenuInputFunction_FUNCTION_TAB_PREVIOUS = 173,
        EMenuInputFunction_FUNCTION_TAB_NEXT = 174,
        EMenuInputFunction_FUNCTION_CYCLE_PREVIOUS = 175,
        EMenuInputFunction_FUNCTION_CYCLE_NEXT = 176,
        EMenuInputFunction_FUNCTION_INBOX = 177,
        EMenuInputFunction_FUNCTION_MENU_OPTIONS = 178,
        EMenuInputFunction_FUNCTION_INGAME_OPTIONS = 179,
        EMenuInputFunction_FUNCTION_INGAME_INTEL = 180,
        EMenuInputFunction_FUNCTION_ANALOG_CLOCKWISE = 181,
        EMenuInputFunction_FUNCTION_ANALOG_COUNTERCLOCKWISE = 182,
        EMenuInputFunction_FUNCTION_ANALOG_RIGHT = 183,
        EMenuInputFunction_FUNCTION_ANALOG_LEFT = 184,
        EMenuInputFunction_FUNCTION_PHOTOMODE_OPEN = 185,
        EButton_BUTTON_TRIANGLE = 186,
        EButton_BUTTON_CIRCLE = 187,
        EButton_BUTTON_CROSS = 188,
        EButton_BUTTON_SQUARE = 189,
        EButton_BUTTON_SHOULDER_LEFT2 = 190,
        EButton_BUTTON_SHOULDER_RIGHT2 = 191,
        EButton_BUTTON_SHOULDER_LEFT1 = 192,
        EButton_BUTTON_SHOULDER_RIGHT1 = 193,
        EButton_BUTTON_START = 194,
        EButton_BUTTON_SELECT = 195,
        EButton_BUTTON_LEFT_ANALOG = 196,
        EButton_BUTTON_RIGHT_ANALOG = 197,
        EButton_BUTTON_UP = 198,
        EButton_BUTTON_RIGHT = 199,
        EButton_BUTTON_DOWN = 200,
        EButton_BUTTON_LEFT = 201,
        EButton_BUTTON_ReservedBit16 = 202,
        EButton_BUTTON_ReservedBit17 = 203,
        EButton_BUTTON_ReservedBit18 = 204,
        EButton_BUTTON_ReservedBit19 = 205,
        EButton_BUTTON_ReservedBit20 = 206,
        EButton_BUTTON_TOUCH_PAD = 207,
        EButton_BUTTON_TOUCH_PAD_LEFT = 208,
        EButton_BUTTON_TOUCH_PAD_RIGHT = 209,
        EButton_BUTTON_TOUCH_PAD_SWIPE_UP = 210,
        EButton_BUTTON_TOUCH_PAD_SWIPE_LEFT = 211,
        EButton_BUTTON_TOUCH_PAD_SWIPE_RIGHT = 212,
        EButton_BUTTON_TOUCH_PAD_SWIPE_DOWN = 213,
        kDSFunctionSystemReserved_Decide = 214,
        kDSFunctionSystemReserved_Cancel = 215,
        kDSFunctionCommon_Decide = 216,
        kDSFunctionCommon_Cancel = 217,
        kDSFunctionCommon_SkipMessage = 218,
        kDSFunctionCommon_MainMenu = 219,
        kDSFunctionCommon_SystemMenu = 220,
        kDSFunctionCommon_ChangeAmmoType = 221,
        kDSFunctionCommon_Shout = 223,
        kDSFunctionCommon_RideAndGetOff = 224,
        kDSFunctionCommon_Sensor = 222,
        kDSFunctionCommon_Marker = 225,
        kDSFunctionCommon_MarkerPrev = 226,
        kDSFunctionCommon_MarkerNext = 227,
        kDSFunctionAction_ADS = 228,
        kDSFunctionAction_Fire = 229,
        kDSFunctionAction_LeftSideAction = 230,
        kDSFunctionAction_RightSideAction = 231,
        kDSFunctionAction_LeftSideActionGrab = 232,
        kDSFunctionAction_RightSideActionGrab = 233,
        kDSFunctionAction_LeftSideActionBrace = 234,
        kDSFunctionAction_RightSideActionBrace = 235,
        kDSFunctionAction_Reload = 236,
        kDSFunctionAction_Melee = 237,
        kDSFunctionAction_Pickup = 238,
        kDSFunctionAction_Access = 239,
        kDSFunctionAction_Action = 240,
        kDSFunctionAction_BreathStop = 241,
        kDSFunctionAction_Sensor = 242,
        kDSFunctionAction_SwitchOdradekLight = 243,
        kDSFunctionAction_InventoryMenu = 244,
        kDSFunctionView_SwitchCamera = 245,
        kDSFunctionView_ZoomIn = 246,
        kDSFunctionView_FirstPersonView = 247,
        kDSFunctionAction_MoveToForward = 248,
        kDSFunctionAction_MoveToBack = 249,
        kDSFunctionAction_MoveToLeft = 250,
        kDSFunctionAction_MoveToRight = 251,
        kDSFunctionAction_ChangeStance = 252,
        kDSFunctionAction_Jump = 253,
        kDSFunctionAction_Sprint = 254,
        kDSFunctionAction_Walk = 255,
        kDSFunctionVehicle_MoveToForward = 256,
        kDSFunctionVehicle_MoveToBack = 257,
        kDSFunctionVehicle_MoveToLeft = 258,
        kDSFunctionVehicle_MoveToRight = 259,
        kDSFunctionVehicle_Accelerator = 260,
        kDSFunctionVehicle_Brake = 261,
        kDSFunctionVehicle_HandBrake = 262,
        kDSFunctionVehicle_Booster = 263,
        kDSFunctionVehicle_Jump = 264,
        kDSFunctionVehicle_Wheelie = 265,
        kDSFunctionVehicle_JackKnife = 266,
        kDSFunctionVehicle_KickFloatingCarrier = 267,
        kDSFunctionVehicle_GetOffFromFloatingCarrier = 268,
        kDSFunctionHUD_WeaponSelector = 269,
        kDSFunctionHUD_MagazineSelector = 270,
        kDSFunctionHUD_EquipmentSelector = 271,
        kDSFunctionHUD_ItemSelector = 272,
        kDSFunctionHUD_SelectorPagePrev = 273,
        kDSFunctionHUD_SelectorPageNext = 274,
        kDSFunctionHUD_SelectItemUseOrEquip = 275,
        kDSFunctionHUD_SelectItemPut = 276,
        kDSFunctionHUD_SelectItemMoveToX = 277,
        kDSFunctionHUD_SelectItemMoveToLeftHand = 278,
        kDSFunctionHUD_SelectItemMoveToRightHand = 279,
        kDSFunctionPrivateRoom_Button0 = 280,
        kDSFunctionPrivateRoom_Button1 = 281,
        kDSFunctionPrivateRoom_Button2 = 282,
        kDSFunctionPrivateRoom_Button3 = 283,
        kDSFunctionPhotoMode_OpenMenu = 284,
        kDSFunctionPhotoMode_SwitchOperationMode = 285,
        kDSFunctionPhotoMode_MoveToUp = 286,
        kDSFunctionPhotoMode_MoveToDown = 287,
        kDSFunctionPhotoMode_Sprint = 288,
        kDSFunctionPhotoMode_Walk = 289,
        kDSFunctionVirtual_Rumble = 290,
        kDSFunctionVirtual_CareBB = 291,
        kDSFunctionVirtual_CameraAction = 292,
        kDSFunctionVirtual_SelectorItemSelect = 293,
        kDSFunctionVirtual_SelectorPageChange = 294,
        kDSFunctionVirtual_MoveToX = 295,
        kDSFunctionVirtual_LookToX = 296,
        kDSFunctionDebug_FlyMode = 297,
        kDSFunctionDebug_LookToUp = 298,
        kDSFunctionDebug_LookToDown = 299,
        kDSFunctionDebug_LookToLeft = 300,
        kDSFunctionDebug_LookToRight = 301,
    }

    [RTTI.Serializable(0x5EDD187E93B62BF, GameType.DS)]
    public enum EDSFunctionKeyBindStickOutputPropertyAxisType : int32
    {
        kNone = 0,
        kX = 1,
        kY = 2,
        kZ = 3,
    }

    [RTTI.Serializable(0xE7C66DBE5F165250, GameType.DS)]
    public enum EDSFunctionKeyBindStickOutputPropertyOverwriteTarget : int32
    {
        kNone = -1,
        kStickLeft = 0,
        kStickRight = 1,
    }

    [RTTI.Serializable(0x26077F56CDFBAD08, GameType.DS)]
    public enum EDSFuzeType : int8
    {
        None = 0,
        TimeFuze = 1,
        PercussionFuze = 2,
        ProximityFuze = 3,
        RemoteFuze = 4,
    }

    [RTTI.Serializable(0xDAD9F5B118A8D56B, GameType.DS)]
    public enum EDSGameActorType : int32
    {
        Player = 0,
        Mule = 1,
        MuleCP = 2,
        Baggage = 3,
        DeliveryPoint = 4,
        MulePost = 5,
        Zipline = 6,
        Catcher = 7,
        Gazer = 8,
        Cart = 9,
        Ladder = 10,
        Bridge = 11,
        FieldRope = 12,
        TracePoint = 13,
        Signboard = 14,
        VehicleMotorbike = 15,
        VehicleTruck = 16,
        ShellLauncher = 17,
        AutoDeliveryCarrier = 18,
        BaggageSystemWarehouse = 19,
        Higgs = 20,
        Airplane = 21,
        FixedGun = 22,
        Cliff = 23,
        Amelie = 24,
        Collectible = 25,
        MissionDeliveredStorage = 26,
        BaggageShelf = 27,
        PrivateRoomStorage = 28,
        AreaChangeStorage = 29,
        PlayerBackup = 30,
        GodsHand = 31,
        Gimmick = 32,
        AnnihilationStrandMarker = 33,
        SupportHunter = 34,
        Hunter = 35,
    }

    [RTTI.Serializable(0x4884582ACBE0E0F8, GameType.DS)]
    public enum EDSGameBaggageListItem_BaggageAttribute : int8
    {
        Locked = 0,
        Personal = 1,
        Dummy = 2,
        Discarded = 3,
        DummyBaggage = 4,
        NonBaggage = 5,
    }

    [RTTI.Serializable(0xF199112DAF6A0C5D, GameType.DS)]
    public enum EDSGameBaggageListItem_BaggageCaseType : int8
    {
        Normal = 0,
        LiquidOnly = 1,
        Weapon = 2,
        Item = 3,
        Equipment = 4,
        BBPod = 5,
        BodyBag = 6,
        Dummy = 7,
        Handcuffs = 8,
        Material = 9,
        Cart = 10,
        ConstractionMachine = 11,
        Ladder = 12,
        Delicate = 13,
        Rope = 14,
        Vehicle = 15,
        LivingThing = 16,
        SmallDelicate = 17,
    }

    [RTTI.Serializable(0xFA16A69EECCF135F, GameType.DS)]
    public enum EDSGameBaggageListItem_ContentsDamageType : int8
    {
        Normal = 0,
        Fragile = 1,
        Delicate = 2,
        Danger = 3,
        SensitiveToTimefall = 4,
        Equipment = 5,
        LivingThing = 6,
        MustKeepHorizontally = 7,
        Cool = 8,
    }

    [RTTI.Serializable(0xA084D8D9B5614220, GameType.DS)]
    public enum EDSGameBaggageListItem_ContentsType : int8
    {
        Commodity = 0,
        Weapon = 1,
        Equipment = 2,
        Special = 3,
        RawMaterial = 4,
    }

    [RTTI.Serializable(0x218C04B87BF7823A, GameType.DS)]
    public enum EDSGameBaggageListItem_Volume : int8
    {
        S = 0,
        M = 1,
        L = 2,
        LL = 3,
        Human = 4,
    }

    [RTTI.Serializable(0xF983FB4390B73E3D, GameType.DS)]
    public enum EDSGameCatalogueListItem_UITabType : int16
    {
        DeliveryMachine = 0,
        Equipment = 1,
        Weapon = 2,
        RawMaterial = 3,
        BackPack = 4,
        Vehicle = 5,
    }

    [RTTI.Serializable(0xBC29E915F08135EC, GameType.DS)]
    public enum EDSGameCatalogueListItem_UnlockDialogType : int8
    {
        Bridges = 0,
        Household = 1,
        Place = 2,
    }

    [RTTI.Serializable(0x6BED32794CA2E8C, GameType.DS)]
    public enum EDSGameCatalogueListItem_UnlockType : int8
    {
        Normal = 0,
        FriendShipUILevel1 = 1,
        FriendShipUILevel2 = 2,
        FriendShipUILevel3 = 3,
        FriendShipUILevel4 = 4,
        FriendShipUILevel5 = 5,
        MissionClear = 6,
        MissionClearOrFriendShipUILevel1 = 7,
        MissionClearOrFriendShipUILevel2 = 8,
        MissionClearOrFriendShipUILevel3 = 9,
        MissionClearOrFriendShipUILevel4 = 10,
        MissionClearOrFriendShipUILevel5 = 11,
        MemoryChip = 12,
        GameClear = 13,
        DeliveryOfLostBaggage = 14,
    }

    [RTTI.Serializable(0xB4D4EDB0A82F767C, GameType.DS)]
    public enum EDSGameCatalogueListItem_UsageType : int8
    {
        None = 0,
        Normal = 1,
        Vehicle = 2,
        Post = 3,
        SafetyHouse = 4,
        BackPackCustomize = 5,
        RawMaterial = 6,
        All = 7,
    }

    [RTTI.Serializable(0xAC2FCC405213B98C, GameType.DS)]
    public enum EDSGameCommodityListItem_ConsumeType : int16
    {
        None = 0,
        Once = 1,
        LikeBattery = 2,
    }

    [RTTI.Serializable(0xCBDA5C3A590CB946, GameType.DS)]
    public enum EDSGameCommodityListItem_EffectivenessType : int16
    {
        Water = 0,
        RecoveryOfStamina = 0,
        IncreaseBlood = 1,
        RecoveryOfBlood = 1,
        RecoveryOfCase = 2,
        RecoveryOfBattery = 3,
        Shoes = 4,
        BT_Light = 5,
    }

    [RTTI.Serializable(0x3EE4CFEBCE6BE162, GameType.DS)]
    public enum EDSGameEquipmentListItem_Type : int16
    {
        Suits = 0,
        Mask = 1,
        Boots = 2,
    }

    [RTTI.Serializable(0xE5C6FCCE046B945A, GameType.DS)]
    public enum EDSGameOverFadeColor : int8
    {
        FadeColorBlack = 0,
        FadeColorWhite = 1,
    }

    [RTTI.Serializable(0x3D2FE118E885C6D9, GameType.DS)]
    public enum EDSGameRawMaterialtListItem_Type : int16
    {
        Crystal = 0,
        Resin = 1,
        Metal = 2,
        Ceramic = 3,
        ChemicalSubstance = 4,
        SpecialAlloy = 5,
        Max = 6,
        A = 1,
        B = 2,
        C = 0,
    }

    [RTTI.Serializable(0xE554D869655807B5, GameType.DS)]
    public enum EDSGameStateForScript : int32
    {
        None = 0,
        TerminalOperation = 1,
        TerminalOperationSimple = 2,
        PauseMenu = 4,
        PrivateRoom = 8,
        BaggageSelect = 16,
        HandcuffDevice = 32,
        Cutscene = 64,
        WeaponSelector = 128,
        Subspace = 256,
        KnotSpace = 512,
        StrideAreaEvent = 1024,
        Result = 2048,
        FastTravel = 4096,
        GameOver = 8192,
        Faint = 16384,
        BossBattle = 32768,
        AfterBeatBoss = 65536,
        Annihilation = 131072,
        Movie = 262144,
        InGameCutscene = 524288,
        InGameActing = 1048576,
        RelocateBaggage = 2097152,
        Loading = 4194304,
        HeartmanRoom = 8388608,
        Renovation = 16777216,
        Onsen = 33554432,
        Setup = 67108864,
        CallingRadio = 134217728,
        SystemPause = 268435456,
        AreaChanging = 536870912,
        DirectIntoPrivateRoom = 1073741824,
    }

    [RTTI.Serializable(0xEDFA02EC6F03ABA, GameType.DS)]
    public enum EDSGameStateForScript2 : int32
    {
        None = 0,
        Night = 1,
        WW1 = 2,
        M650Beach = 4,
        HiggsFistfight = 8,
        AllowPathRecord = 16,
        PathRecordStraightMode = 32,
        CancelMission = 64,
        EventsRestricted = 128,
        StayPrivateRoom = 256,
        RainSheleterSkipTime = 512,
        WaitCloseLoadingScreen = 1024,
        PlayerDied = 2048,
        PlayerFainted = 4096,
        HUDHideImmediately = 8192,
        HUDHideFadeout = 16384,
        SignboardMenu = 32768,
        ExplosionObjectSuspendRequest = 65536,
        ExplosionObjectRemoveRequest = 131072,
        CanMoveCameraForInGameCutscene = 131072,
        StaySubspace = 524288,
        MemoriesOfCliff = 1048576,
        EndingStaffRoll = 2097152,
        HeavyCutscene = 4194304,
        ExplosionObjectRemoveRequestForBeforeExplosion = 8388608,
        ExplosionObjectRemoveRequestForDetonatableState = 16777216,
        CutsceneNoDelay = 33554432,
    }

    [RTTI.Serializable(0x9923D1733C022838, GameType.DS)]
    public enum EDSGameWeaponListItem_Category : int16
    {
        Gun = 0,
        Thorwing = 1,
        Placement = 2,
    }

    [RTTI.Serializable(0x72587E38D9949AB5, GameType.DS)]
    public enum EDSGameWeaponListItem_Type : int16
    {
        Main = 0,
        Sub = 1,
    }

    [RTTI.Serializable(0xCDFBE3629EE425B, GameType.DS)]
    public enum EDSGazerBolagunAnimationType : int8
    {
        BodyF1 = 18,
        BodyB1 = 20,
        LegF1 = 17,
        LegB1 = 19,
        BodyF2 = 22,
        BodyB2 = 24,
        LegF2 = 21,
        LegB2 = 23,
        Navel = 25,
    }

    [RTTI.Serializable(0xE795AEDBEC42014F, GameType.DS)]
    public enum EDSGazerBoneType : int8
    {
        Normal = 0,
        Navel = 1,
    }

    [RTTI.Serializable(0x99C297113C9EA687, GameType.DS)]
    public enum EDSGazerMeshType : int8
    {
        Head = 0,
        BodyLA = 1,
        BodyLB = 2,
        BodyLC = 3,
        BodyRA = 4,
        BodyRB = 5,
        BodyRC = 6,
        ArmLA = 7,
        ArmLB = 8,
        ArmRA = 9,
        ArmRB = 10,
        LegLA = 11,
        LegLB = 12,
        LegRA = 13,
        LegRB = 14,
    }

    [RTTI.Serializable(0x1403F49D39D3F1CF, GameType.DS)]
    public enum EDSHUDBlinkType : int8
    {
        OneBlink = 0,
        LoopBlink = 1,
        OpenToKeepClosing = 2,
        OpenToKeepClosingToOpen = 3,
        KeepClosingToOpen = 4,
        CustomCurve = 5,
    }

    [RTTI.Serializable(0x2629F4D8921DCA49, GameType.DS)]
    public enum EDSHeartmanTimerOperationSequenceEventCaptionType : int32
    {
        ToRevivalCaption = 0,
        ToDeathCaption = 1,
    }

    [RTTI.Serializable(0x644D512D3EF62513, GameType.DS)]
    public enum EDSHiggsVoicePlayType : int8
    {
        Random = 0,
        Sequence = 1,
    }

    [RTTI.Serializable(0x56479113259ADE00, GameType.DS)]
    public enum EDSIntArgIndex : int32
    {
        Index0 = 0,
        Index1 = 1,
        Index2 = 2,
        Index3 = 3,
    }

    [RTTI.Serializable(0xF19A3D6DE54CDE89, GameType.DS)]
    public enum EDSIntReturnValueIndex : int32
    {
        Index0 = 0,
        Index1 = 1,
        Index2 = 2,
        Index3 = 3,
    }

    [RTTI.Serializable(0x987FF9F77E039E17, GameType.DS)]
    public enum EDSInterviewGroup : int8
    {
        Unknown = 0,
        Diehardman = 1,
        Deadman = 2,
        Heartman = 3,
        Mama = 4,
        Lockne = 5,
        Fragile = 6,
        BridgesStaff = 7,
        Preppers = 8,
        HiggesNote = 9,
        LucyReport = 10,
        OldDiehardman = 11,
        Other = 12,
    }

    [RTTI.Serializable(0xB596752807EB7FBC, GameType.DS)]
    public enum EDSItemId : int8
    {
        None = 0,
        Water = 1,
        Food = 2,
        Coating = 3,
        Battery = 4,
        Insecticide = 5,
        BloodPack = 6,
        BTLight = 7,
        Odradek = 8,
        ShoesA = 9,
        PostBuilder = 10,
        ZiplineBuilder = 11,
        BridgeBuilder = 12,
        CapsuleShelterBuilder = 13,
        ShoeSoleGrass = 14,
        NormalBoots = 15,
        StableBootsLv1 = 16,
        StableBootsLv2 = 17,
        StableBootsLv3 = 18,
        MuleBoots = 19,
        TerroristBoots = 20,
        ChiralCylinder = 21,
        Cryptobiosys = 22,
        BloodPackLarge = 23,
        ChiralBoots = 24,
        WaterLv2 = 25,
        CryptobiosisD = 26,
    }

    [RTTI.Serializable(0x4F8C5C9163BA1F5C, GameType.DS)]
    public enum EDSKeyBindCategory : int8
    {
        ActionGamePad = 0,
        Action = 1,
        Vehicle = 2,
        HUD = 3,
        Menu = 4,
        MenuMap = 5,
        ConstructionPoint = 6,
        None = 7,
    }

    [RTTI.Serializable(0x7E02672B20CF5ABB, GameType.DS)]
    public enum EDSKeyBindKeyboardLayout : int32
    {
        kNone = 0,
        kQWERTY = 1,
        kAZERTY = 2,
    }

    [RTTI.Serializable(0xBE42952FC5DFE169, GameType.DS)]
    public enum EDSKeyBindLayer : int8
    {
        kNone = 0,
        kSystem = 1,
        kFloatingCart = 2,
        kMenu = 3,
        kHUD = 4,
        kUI = 5,
        kPrivateRoom = 6,
    }

    [RTTI.Serializable(0xC3CCC3E772F18A1B, GameType.DS)]
    public enum EDSKeyBindPlatform : int32
    {
        kNone = 0,
        kPC = 1,
        kPS4 = 2,
        kPS5 = 3,
    }

    [RTTI.Serializable(0x7B20E5C141DA70CB, GameType.DS)]
    public enum EDSKnowledgeType : uint8
    {
        DSKnowledge_None = 0,
        DSKnowledge_PlayerVisual = 1,
        DSKnowledge_EnemyVisual = 2,
        DSKnowledge_BaggageVisual = 3,
        DSKnowledge_GeneralVisual = 5,
        DSKnowledge_DamagedFriendVisual = 4,
        DSKnowledge_SmokeVisual = 6,
        DSKnowledge_PlayerNoise = 51,
        DSKnowledge_EnemyNoise = 52,
        DSKnowledge_FriendNoise = 53,
        DSKnowledge_CautionNoise = 54,
        DSKnowledge_Damaged = 81,
        DSKnowledge_RaderBaggage = 82,
        DSKnowledge_CPOrder = 83,
        DSKnowledge_PlayerLastKnown = 84,
        DSKnowledge_PlayerIsNonresistance = 85,
        DSKnowledge_AngerToPlayer = 86,
        DSKnowledge_BaggageStickied = 87,
        DSKnowledge_Timefall = 90,
        DSKnowledge_Aimed = 91,
        DSKnowledge_InCombat = 92,
        DSKnowledge_PlayerUnfavorable = 93,
        DSKnowledge_ObstacleInCurrentPath = 94,
        DSGazerKnowledge_Breath = 128,
        DSGazerKnowledge_Touch = 129,
        DSGazerKnowledge_Waste = 130,
        DSGazerKnowledge_Npc = 131,
        DSGazerKnowledge_LastKnown = 132,
        DSGazerKnowledge_PlayerNoise = 133,
        DSMuleKnowledge_AttackedBT = 134,
        DSMuleKnowledge_NearBT = 135,
        DSMuleKnowledge_HasTemporaryBaggage = 136,
        DSMuleKnowledge_NeighborVisual = 137,
        DSMuleKnowledge_ReceivedLike = 138,
        DSMuleKnowledge_FakeCamouf = 139,
        DSWarriorsKnowledge_LostWarrior = 140,
        DSWarriorsKnowledge_SummonWarrior = 141,
        DSKnowledge_PlayerNoiseRepete = 143,
        DSKnowledge_PlayerNoiseRepete2 = 144,
        DSKnowledge_PlayerGotDown = 145,
        DSKnowledge_AimedVisual = 146,
        DSKnowledge_AlertNoise = 147,
        DSKnowledge_GrenadeSign = 148,
        DSKnowledge_LastIndis = 149,
        DSKnowledge_RaderBaggageRepete = 150,
        DSKnowledge_DamagedBulletRepete = 151,
    }

    [RTTI.Serializable(0x697AF366012BF8DC, GameType.DS)]
    public enum EDSLadderType : int8
    {
        Normal = 0,
        NetNormal = 1,
        StageNormal = 2,
    }

    [RTTI.Serializable(0xF6EDD328A62AF64C, GameType.DS)]
    public enum EDSLiftLockReason : int32
    {
        CorpseBag = 1,
        NotEnoughCapacity = 2,
        MoreThanTwoVehicles = 4,
        MuleVehicle = 8,
    }

    [RTTI.Serializable(0xB69AC78FA1F54D8, GameType.DS)]
    public enum EDSLikeTargetType : int8
    {
        Baggage = 0,
        Post = 1,
        Vehicle = 2,
        Cart = 3,
        Safetyhouse = 4,
        Bridge = 5,
        Road = 6,
        ZipLine = 7,
        Ladder = 8,
        FieldRope = 9,
        Charger = 10,
        RainShelter = 11,
        Comment = 12,
        Signboard = 13,
        WatchTower = 14,
        __0 = 15,
        PlayerTracePoint = 16,
        FriendlyMule = 17,
        SupportHunter = 18,
        SupportHunterBaggage = 19,
        Highway = 20,
        Mushroom = 21,
        Supply = 22,
        TakeShared = 23,
        RecycleMaterial = 24,
        None = 25,
    }

    [RTTI.Serializable(0x4845EE6BDF5FFBBB, GameType.DS)]
    public enum EDSListItemColor : int8
    {
        Red = 0,
        Yellow = 1,
        Blue = 2,
        Gray = 3,
        Orange = 4,
        Purple = 5,
    }

    [RTTI.Serializable(0x705ABEEB9743AC45, GameType.DS)]
    public enum EDSLockType : int8
    {
        Normal = 0,
        Blood = 1,
        All = 2,
    }

    [RTTI.Serializable(0x42E7F5E25DAAD0A7, GameType.DS)]
    public enum EDSMagazineId : uint8
    {
        None = 0,
        AssaultRifle = 1,
        AssaultRifleLv2 = 2,
        AssaultRifleLv3 = 3,
        AssaultRifleLv4 = 4,
        AssaultRifleBloodBullet = 5,
        AssaultRifleLv2BloodBullet = 6,
        AssaultRifleLv3BloodBullet = 7,
        AssaultRifleLv4BloodBullet = 8,
        AssaultRifleGoldenBullet = 9,
        AssaultRifleLv2GoldenBullet = 10,
        AssaultRifleLv3GoldenBullet = 11,
        AssaultRifleLv4GoldenBullet = 12,
        Grenade = 13,
        BloodGrenade = 14,
        BloodGrenadeLv1Extend = 15,
        BloodGrenadeLv2 = 16,
        ElectricalGrenadeLv1 = 17,
        ElectricalGrenadeLv2 = 18,
        ElectricalGrenadePlace = 19,
        CoatingSpray = 20,
        SmokeGrenade = 21,
        SmokeGrenadeLv2 = 22,
        FreezeGrenade = 23,
        TranquilizerGun = 24,
        AmnioticFluidGrenade = 25,
        ExGrenade0 = 26,
        ExGrenade1 = 27,
        ExGrenade1Plus = 28,
        ExGrenade2 = 29,
        BolaGun = 30,
        BolaGunLv2 = 31,
        ShotGun = 32,
        ShotGunLv2 = 33,
        ShotGunLv3 = 34,
        ShotGunBloodBullet = 35,
        ShotGunBloodBulletLv2 = 36,
        ShotGunBloodBulletLv3 = 37,
        ShotGunGoldenBulletLv3 = 38,
        HandGun = 39,
        HandGunLv2 = 40,
        HandGunLv3 = 41,
        HandGunBloodBullet = 42,
        HandGunBloodBulletLv2 = 43,
        HandGunBloodBulletLv3 = 44,
        HandGunGoldenBullet = 45,
        HandGunGoldenBulletLv2 = 46,
        HandGunGoldenBulletLv3 = 47,
        BloodHandGun = 48,
        BloodHandGunLv2 = 49,
        AmelieHandGun = 50,
        C4 = 51,
        GazerBalloon = 52,
        SamBall = 53,
        SamBallLv2 = 54,
        GrenadeShell = 55,
        BloodGrenadeShell = 56,
        SlipGrenadeShell = 57,
        AmnioticFluidGrenadeShell = 58,
        TranquilizerGrenadeShell = 59,
        SubGrenadeShell = 60,
        SubBloodGrenadeShell = 61,
        SubSlipGrenadeShell = 62,
        SubAmnioticFluidGrenadeShell = 63,
        SubTranquilizerGrenadeShell = 64,
        AssaultRifleRubberBullet = 65,
        AssaultRifleRubberBulletLv2 = 66,
        AssaultRifleRubberBulletLv3 = 67,
        AssaultRifleRubberBulletLv4 = 68,
        AssaultRifleRubberBloodBullet = 69,
        AssaultRifleRubberBloodBulletLv2 = 70,
        AssaultRifleRubberBloodBulletLv3 = 71,
        AssaultRifleRubberBloodBulletLv4 = 72,
        AssaultRifleRubberGoldenBulletLv3 = 73,
        AssaultRifleRubberGoldenBulletLv4 = 74,
        ShotGunRubberBullet = 75,
        ShotGunRubberBulletLv2 = 76,
        ShotGunRubberBulletLv3 = 77,
        ShotGunRubberBloodBullet = 78,
        ShotGunRubberBloodBulletLv2 = 79,
        ShotGunRubberBloodBulletLv3 = 80,
        ShotGunRubberGoldenBulletLv3 = 81,
        PostBuilder = 82,
        ZiplineBuilder = 83,
        CapsuleShelterBuilder = 84,
        BridgeBuilder = 85,
        SafetyHouseBuilder = 86,
        WatchTowerBuilder = 87,
        __0 = 88,
        ChargerBuilder = 89,
        RainShelterBuilder = 90,
        CamouflageBuilder = 91,
        Ladder = 92,
        Rope = 93,
        StickyBullet = 94,
        FourConsecutiveMissile = 95,
        FourConsecutiveMissileBlood = 96,
        SpreadMissile = 97,
        SpreadMissileBlood = 98,
        SpreadMissileChild = 99,
        SpreadMissileChildBlood = 100,
        HologramDevice = 101,
        EnemyAssaultRifle = 102,
        MultiRod = 103,
        EnemySlipGrenadeShell = 104,
        EnemyAssaultRifleRubberBullet = 105,
        HiggsAssaultRifleBullet = 106,
        Ww1Rifle = 107,
        Ww1ShotGun = 108,
        Ww1Grenade = 109,
        Ww1MachineGun = 110,
        Ww2SubmachineGun = 111,
        Ww2Rifle = 112,
        Ww2Missile = 113,
        Ww2MissileType2 = 114,
        Ww2SmokeGrenade = 115,
        VietnamAssault = 116,
        VietnamAssaultWithGrenader = 117,
        VietnamAssaultWithGrenaderShell = 118,
        VietnamMachinegun = 119,
        VietnamGrenade = 120,
        CliffRifle = 121,
        AfghanRifle = 122,
        HiggsKnife = 123,
        DemensShotGunBullet = 125,
        DemensAssaultRifleBullet = 124,
        EnemyGrenade = 126,
        Ww2Grenade = 127,
        AfghanGrenade = 128,
        __1 = 129,
        Ww2AirPlaneMachinegun = 130,
        Ww2HeavyMachinegun = 131,
        RdGrenadeShell = 132,
        RdBloodGrenadeShell = 133,
        RdSlipGrenadeShell = 134,
        RdTranquilizerGrenadeShell = 135,
        DemensElectricalGrenade = 136,
        __2 = 137,
    }

    [RTTI.Serializable(0x7708B0544021C1C9, GameType.DS)]
    public enum EDSMailInfoStatus : int8
    {
        UnSent = 0,
        Sent = 1,
        ReciveUnopend = 2,
        ReciveOpened = 3,
    }

    [RTTI.Serializable(0x6FBF26105EC628AE, GameType.DS)]
    public enum EDSMajorMember : int8
    {
        None = 0,
        Diehardman = 1,
        Deadman = 2,
        Heartman = 3,
        Mama = 4,
        Lockne = 5,
        Fragile = 6,
        BB = 7,
        BT = 8,
        BridgesOrganization = 9,
        PorterMule = 10,
        ChiralArtistDaughter = 11,
        Langdon = 12,
    }

    [RTTI.Serializable(0x44AFAB68FD4D9FA9, GameType.DS)]
    public enum EDSMarkerLineEffectShapeType : int32
    {
        PolyLine = 0,
        NavigationLine = 1,
    }

    [RTTI.Serializable(0xB446A03FFE51FFE1, GameType.DS)]
    public enum EDSMarkerLocatorType : int8
    {
        Common = 0,
        ZiplineSpot = 1,
    }

    [RTTI.Serializable(0x767A866038789533, GameType.DS)]
    public enum EDSMaterialConfigType : int16
    {
        NONE = 0,
        NORMAL = 1,
        WATER = 2,
        ROCK = 3,
        MOSS = 4,
        TAR = 5,
        ROAD = 6,
        SlipGrenade = 7,
        Snow = 8,
        Mud = 9,
        Sand = 10,
        Deblis = 11,
        Ice = 12,
        SnowShallow = 13,
    }

    [RTTI.Serializable(0xF86435DBB6CDC128, GameType.DS)]
    public enum EDSMenuRadioFactType : int8
    {
        ReadWrite = 0,
        ReadOnly = 1,
        WriteOnly = 2,
    }

    [RTTI.Serializable(0x35DA7FF70D4BA398, GameType.DS)]
    public enum EDSMessageFromResultGraphService : int8
    {
        AllDispResult = 0,
        DispResult1 = 1,
        DispResult2 = 2,
    }

    [RTTI.Serializable(0x75C4BB15F4366EC6, GameType.DS)]
    public enum EDSMissionAttrFlag : int32
    {
        None = 0,
        CompleteToDeliveryAtOne = 8,
    }

    [RTTI.Serializable(0xE365A26A0CA43C7E, GameType.DS)]
    public enum EDSMissionCategory : int16
    {
        MainMission = 0,
        SubMission = 1,
        SimpleMission = 2,
        FreeMission = 3,
    }

    [RTTI.Serializable(0xE565B86D5271D73A, GameType.DS)]
    public enum EDSMissionCategoryDetail : int8
    {
        MainMission = 0,
        SubMission = 1,
        StaticSimpleDeliveryMission = 2,
        StaticSimpleCollectMission = 3,
        OnlineLostMission = 4,
        OnlineSupplyMission = 5,
        DynamicOfflineLostMission = 6,
        FreeMission = 7,
    }

    [RTTI.Serializable(0x4F466C4DFF108EB4, GameType.DS)]
    public enum EDSMissionConditionType : int8
    {
        Unknown = 0,
        Safety = 1,
        Speed = 2,
        BaggageCount = 3,
        BaggageWeight = 4,
        Service = 5,
        ChiralDelivery = 6,
        Damage = 7,
    }

    [RTTI.Serializable(0x7020B4F693CEB90A, GameType.DS)]
    public enum EDSMissionGameOverType : int8
    {
        AllDream = 0,
        BlackSam = 1,
    }

    [RTTI.Serializable(0xC12D72E20E564824, GameType.DS)]
    public enum EDSMissionGoalsNotificationReason : int8
    {
        ToShowCurrentShortTermGoal = 0,
        ToShowCurrentLongTermGoal = 1,
        ToRemindCurrentGoalsAndTodos = 2,
    }

    [RTTI.Serializable(0xEA3CA7D7D17C73BD, GameType.DS)]
    public enum EDSMissionLogicBlockType : int8
    {
        LogicBlockAnd = 1,
        LogicBlockOr = 2,
    }

    [RTTI.Serializable(0x6303241C69EC4A6F, GameType.DS)]
    public enum EDSMissionMsgGraphExecMode : int8
    {
        CatchMissionSystemAndMissionState = 0,
        ExecMissionGraph = 2,
        ExecInProgressMissionGraph = 3,
        ExecCommonGraph = 4,
        CatchMissionSystemOnly = 1,
    }

    [RTTI.Serializable(0x7CD2B4D3FFCC3EFF, GameType.DS)]
    public enum EDSMissionOpenState : int8
    {
        None = 0,
        Displayable = 1,
        NotSelectable = 2,
        NotDisplayable = 8,
    }

    [RTTI.Serializable(0xC6AC4D96F8316C5E, GameType.DS)]
    public enum EDSMissionOrderState : int8
    {
        None = 0,
        NotStarted = 1,
        NotCompleted = 2,
        Completed = 4,
    }

    [RTTI.Serializable(0x768B468FD10BF7CE, GameType.DS)]
    public enum EDSMissionReasonGameOver : int8
    {
        ReasonUnknown = 0,
        ReasonBrokenBaggages = 1,
        ReasonPlayerDied = 2,
        ReasonAnnihilation = 3,
        ReasonBossAnnihilation = 4,
        ReasonTimeOut = 5,
        ReasonNuclearBomb = 6,
        ReasonNuclearBombStolenByMule = 7,
        ReasonPlayerDiedByGoldHunter = 8,
        ReasonGasFaint = 9,
        ReasonKillingBaggage = 10,
        ReasonStolenBaggage = 11,
        ReasonAttackBaggage = 12,
        ReasonGazerAnnihilation = 13,
        ReasonGazerFarAnnihilation = 14,
        ReasonNuclearBombPassingSensor = 15,
        ReasonAnnihilationWithBaggage = 16,
    }

    [RTTI.Serializable(0x732FF40B64C39240, GameType.DS)]
    public enum EDSMissionRecommend : int8
    {
        None = 0,
        Ladder = 1,
        Rope = 2,
        ObservationTower = 3,
        Bridge = 4,
        ZipLine = 5,
    }

    [RTTI.Serializable(0x349B17E7D8F5F438, GameType.DS)]
    public enum EDSMissionRisk : int8
    {
        None = 0,
        Mule = 1,
        Gazer = 2,
        Rain = 3,
        SteepSlope = 4,
        RoughRoad = 5,
        Cliff = 6,
        River = 7,
        Rockfall = 8,
        ToxicGas = 9,
    }

    [RTTI.Serializable(0xC3C1A63DA95BD3C9, GameType.DS)]
    public enum EDSMissionSpecialReportArgument : int8
    {
        Unset = 0,
        MinCountBaggage = 1,
        PremiumMinCountBaggage = 2,
        RankSCountBaggage = 3,
        PremiumRankSCountBaggage = 4,
        CurrentCountBaggage = 5,
        MinWeightBaggage = 6,
        PremiumMinWeightBaggage = 7,
        RankSWeightBaggage = 8,
        PremiumRankSWeightBaggage = 9,
        CurrentWeightBaggage = 10,
        MinTime = 11,
        PremiumMinTime = 12,
        RankSTime = 13,
        PremiumRankSTime = 14,
        CurrentTime = 15,
        MinDistance = 16,
        CurrentDistance = 17,
        DeviationDistanceRate = 18,
        MinTotalBaggageDamageRate = 19,
        PremiumMinTotalBaggageDamageRate = 20,
        RankSTotalBaggageDamageRate = 21,
        PremiumRankSTotalBaggageDamageRate = 22,
        CurrentTotalBaggageDamageRate = 23,
        TotalDeliveryCount = 24,
        TotalDeliveryWeight = 25,
        RouteDeviation = 26,
        Damage = 27,
        MinDamage = 28,
        PremiumMinDamage = 29,
        CustomParam01 = 30,
        CustomParam02 = 31,
    }

    [RTTI.Serializable(0xC6F81C02A63C6A70, GameType.DS)]
    public enum EDSMissionSpecialReportMenuViewType : int8
    {
        Default = 0,
        Notes = 1,
        Must = 2,
        Premium = 3,
    }

    [RTTI.Serializable(0x719A993548198EDA, GameType.DS)]
    public enum EDSMissionSpecialReportOptionAttrFlag : int8
    {
        None = 0,
        IsMissionMenuView = 1,
        IsResultPurposeScoreDownDecoration = 8,
        IsResultPurposeScoreUpDecoration = 16,
    }

    [RTTI.Serializable(0x4F0F2815CCE16D97, GameType.DS)]
    public enum EDSMissionSwitchSectionReason : int8
    {
        None = 0,
        MissionStep = 1,
        StartReplayWarrior = 2,
    }

    [RTTI.Serializable(0x1B6FB1D329CA69E8, GameType.DS)]
    public enum EDSMissionType : int16
    {
        Delivery = 0,
        Collect = 1,
        LostObject = 2,
        Supply = 3,
        Special = 4,
        Free = 5,
    }

    [RTTI.Serializable(0x416C960A9745855A, GameType.DS)]
    public enum EDSMuleAttackType : int32
    {
        Push = 0,
        Kick = 2,
        None = 5,
    }

    [RTTI.Serializable(0xF46EE02C32BCCBBC, GameType.DS)]
    public enum EDSMuleOdradekAction : int8
    {
        Inactive = 0,
        ActiveSonar = 1,
        Siren = 2,
        Miss = 3,
        StaggerL = 4,
        StaggerR = 5,
    }

    [RTTI.Serializable(0x440C5646FBDA1391, GameType.DS)]
    public enum EDSMuleOdradekLightState : int32
    {
        Sneak = 0,
        Caution = 1,
        Notice = 2,
        Alert = 3,
        AlertPlayerLost = 4,
        Evasion = 5,
        Friendly = 6,
    }

    [RTTI.Serializable(0xBC482FAD01570321, GameType.DS)]
    public enum EDSMulePhase : int32
    {
        Mule_NormalPhase = 0,
        Mule_PreCautionPhase = 2,
        Mule_CautionPhase = 3,
        Mule_ReturnPhase = 1,
        Mule_EvasionPhase = 4,
        Mule_AlertPhase = 5,
    }

    [RTTI.Serializable(0x5D552169CACC987E, GameType.DS)]
    public enum EDSMuleStance : int32
    {
        Mule_Stance_Sneak = 0,
        Mule_Stance_Caution = 1,
        Mule_Stance_Alert = 2,
    }

    [RTTI.Serializable(0x1917623593BD1CF8, GameType.DS)]
    public enum EDSMuleType : int32
    {
        Normal = 0,
        Friendly = 1,
        Demens = 2,
        Warriors_Particle = 3,
        Warriors_Battle = 4,
    }

    [RTTI.Serializable(0x1AEE12FF771B3E86, GameType.DS)]
    public enum EDSNetCommentBasePhraseCategory : int8
    {
        Admiration = 0,
        Warning = 1,
        Request = 2,
        Strategy = 3,
        Relation = 4,
        Area = 5,
        Direction = 6,
        Combination = 7,
    }

    [RTTI.Serializable(0xC052163EF2F166FA, GameType.DS)]
    public enum EDSNetCommentOptionPhraseCategory : int8
    {
        All = 0,
        Enemy = 1,
        Building = 2,
        Action = 3,
        Strategy = 4,
        Vehicle = 5,
        Npc = 6,
        Area = 7,
        Direction = 8,
        Feeling = 9,
    }

    [RTTI.Serializable(0xFABF31CFCC631A28, GameType.DS)]
    public enum EDSNetItemPreparationPurpose : int8
    {
        PublicBox = 0,
        SupportHunter = 1,
        Helper0 = 2,
        Helper1 = 3,
        Helper2 = 4,
        Helper3 = 5,
        NightmareHelper0 = 6,
        NightmareHelper1 = 7,
        NightmareHelper2 = 8,
        NightmareHelper3 = 9,
        LastStrandingHelper0 = 10,
        LastStrandingHelper1 = 11,
        LastStrandingHelper2 = 12,
        LastStrandingHelper3 = 13,
        Helper4 = 14,
        NightmareHelper4 = 15,
        LastStrandingHelper4 = 16,
    }

    [RTTI.Serializable(0x89A47689039D875A, GameType.DS)]
    public enum EDSNetSyncObjectState : int8
    {
        Downloaded = 0,
        Activating = 1,
        Activated = 2,
        Fail = 4,
        AlwaysFail = 5,
        Removed = 6,
        Zombie = 3,
    }

    [RTTI.Serializable(0x418EA0CB08A36BB5, GameType.DS)]
    public enum EDSNetSyncObjectType : int8
    {
        Post = 0,
        Zipline = 1,
        Charger = 2,
        __0 = 3,
        SafetyHouse = 4,
        WatchTower = 5,
        Bridge = 6,
        RainShelter = 7,
        FieldRope = 8,
        Ladder = 9,
        VehicleBike = 10,
        VehicleTruck = 11,
        Signboard = 12,
        NationalRoute = 13,
        PileStone = 14,
        Hologram = 15,
        DeathPoint = 16,
        Road = 17,
        PersonalBaggage = 18,
        DeliveryBaggage = 19,
        SupplyMission = 20,
        Mushroom = 21,
    }

    [RTTI.Serializable(0xC9A1BB7F102B3B7C, GameType.DS)]
    public enum EDSNetworkServiceState : int8
    {
        InOperation = 0,
        Maintenance = 1,
        Termination = 2,
    }

    [RTTI.Serializable(0x20B31BF1B7405E35, GameType.DS)]
    public enum EDSNoticeAndKnowledgeCharacterType : int8
    {
        Type_Mule = 0,
        Type_Catcher = 1,
        Type_Gazer = 2,
        Type_Warriors = 3,
        Type_Higgs = 4,
        Type_Cliff = 5,
    }

    [RTTI.Serializable(0x1D5987ED6A2458EA, GameType.DS)]
    public enum EDSNoticeType : uint8
    {
        DSNotice_Discovery = 0,
        DSNotice_Indistinct = 1,
        DSNotice_Dim = 2,
        DSNotice_NoiseAlert = 3,
        DSNotice_NoiseCaution = 4,
        DSNotice_Noise = 5,
        DSNotice_Damaged = 6,
        DSNotice_FriendDamage = 7,
        DSNotice_GenericEvent = 8,
        DSNotice_RaderBaggage = 9,
        DSNotice_DiscoveryBaggage = 10,
        DSNotice_PlayerGotDown = 11,
        DSNotice_Alert = 12,
        DSNotice_Caution = 13,
        DSNotice_NeedSonar = 14,
        DSNotice_Timefall = 15,
        DSNotice_SeeSmoke = 16,
        DSGazerNotice_NewKnowledge = 128,
        DSMuleNotice_AttackedBT = 129,
        DSMuleNotice_ObstacleInCurrentPath = 130,
        DSMuleNotice_DeliverBaggage = 131,
        DSMuleNotice_SeeNeighbor = 132,
        DSMuleNotice_ReceivedLike = 133,
        DSMuleNotice_DiscoveryFakeCamouf = 134,
        DSNotice_FriendDamageLowPriority = 17,
        DSNotice_NoiseEcho = 18,
        DSNotice_WillBeRunOver = 19,
        DSNotice_Grenade = 20,
    }

    [RTTI.Serializable(0x9F74FAF24FE5F70C, GameType.DS)]
    public enum EDSNpcAimType : int8
    {
        UpperBody = 0,
        HeadOnly = 1,
        Torso = 2,
    }

    [RTTI.Serializable(0x797ABA23E07B8DBC, GameType.DS)]
    public enum EDSNpcDefeatType : int32
    {
        Dead = 0,
        Faint = 1,
        Sleep = 2,
        Restrained = 3,
        Dying = 4,
    }

    [RTTI.Serializable(0x4DE2EFF19619C347, GameType.DS)]
    public enum EDSOrganizationGroup : int8
    {
        None = 0,
        Preppers = 1,
        Bridges = 2,
    }

    [RTTI.Serializable(0xF496A3063A07779A, GameType.DS)]
    public enum EDSOutlineEffectMode : int8
    {
        None = 0,
        Outline = 1,
        OutlineGlow = 2,
        OutlineWire = 3,
        OutlinePenetration = 4,
        OutlineGlowPenetration = 5,
        OutlineWirePenetration = 6,
        OutlineStretchNoneCull = 7,
    }

    [RTTI.Serializable(0xF9DC3674A1E88099, GameType.DS)]
    public enum EDSPadGameButtonType : int32
    {
        STANCE = 0,
        DODGE = 1,
        SUB_ACTION_LONG = 2,
        ACTION = 3,
        ACCESS = 4,
        MELEE = 5,
        RIDE_GETOFF = 6,
        SUBJECTIVE = 7,
        FIRE = 8,
        STOCK = 9,
        PICKUP = 10,
        HOLD = 11,
        DASH = 12,
        PICKUP_R = 13,
        PICKUP_L = 14,
        USE_SUITPARTS = 15,
        USE_BAGGAGE_SENSOR = 16,
        SELECT = 17,
        OPTIONS = 18,
        BACK = 19,
        PAD_LEFT = 20,
        PAD_RIGHT = 21,
        PAD_UP = 22,
        PAD_DOWN = 23,
        BREATH_STOP = 24,
        ZOOM = 25,
        FIGHT = 26,
        SELECT_MAIN_WEAPON = 27,
        SELECT_MAGAZINE = 28,
        SELECT_SUIT_PARTS = 29,
        SELECT_ITEM = 30,
        SELECT_EQUIPMENT = 31,
        CHECK = 32,
        DETAIL = 33,
        TAB_BACK = 34,
        TAB_NEXT = 35,
        OK = 36,
        CANCEL = 37,
        DPAD_LEFT = 38,
        DPAD_RIGHT = 39,
        DPAD_UP = 40,
        DPAD_DOWN = 41,
        L1 = 42,
        L2 = 43,
        L3 = 44,
        R1 = 45,
        R2 = 46,
        R3 = 47,
        PRIVATE_ROOM_BUTTON_0 = 48,
        PRIVATE_ROOM_BUTTON_1 = 49,
        PRIVATE_ROOM_BUTTON_2 = 50,
        PRIVATE_ROOM_BUTTON_3 = 51,
        COMMON_MARKER = 52,
        COMMON_DECIDE = 53,
        COMMON_CANCEL = 54,
        COMPASS_FOCUS_LR = 55,
        L_STICK = 56,
        R_STICK = 57,
        L_STICK_LEFT = 58,
        L_STICK_RIGHT = 59,
        L_STICK_UP = 60,
        L_STICK_DOWN = 61,
        R_STICK_LEFT = 62,
        R_STICK_RIGHT = 63,
        R_STICK_UP = 64,
        R_STICK_DOWN = 65,
        R_STICK_ALL_DIR = 66,
        UP_DOWN = 67,
        SHAKE = 68,
        SHAKE_BY_KEYBOARD = 69,
        L_STICK_ROTATE = 70,
        R_STICK_ROTATE = 71,
        NONE = 72,
    }

    [RTTI.Serializable(0x2C9668D7599B4F47, GameType.DS)]
    public enum EDSParkingMenuInfoResult : int32
    {
        StoreVehicle = 1,
        BuyVehicle = 2,
        LeaveVehicle = 4,
        RemoveVehicle = 8,
        RepairVehicle = 16,
    }

    [RTTI.Serializable(0xDFC80163BFD9D650, GameType.DS)]
    public enum EDSPatrolActionObjectLocatorType : int16
    {
        Chair = 0,
        Chest = 1,
    }

    [RTTI.Serializable(0xEB89882B9C2BD9BA, GameType.DS)]
    public enum EDSPatrolFormationType : int8
    {
        _4ForwardFanShape = 0,
        _4ForwardColumn = 1,
        _4SideLine = 2,
        _6Diamond = 3,
        _6SideGroup = 4,
    }

    [RTTI.Serializable(0x9CA9E1585758BFD1, GameType.DS)]
    public enum EDSPatrolLookatSpecialRule : int8
    {
        None = 0,
        OnlyWhenGoingForth = 1,
        OnlyWhenGoingBack = 2,
    }

    [RTTI.Serializable(0x95F6E90C8B0807BF, GameType.DS)]
    public enum EDSPatrolMoveType : int8
    {
        Walk = 0,
        Run = 1,
        WalkLookAround = 2,
        Sprint = 3,
        AlertWalkLookAround = 4,
        FireWalk = 5,
        AutoRunWalk = 6,
        FireWalkLookAround = 7,
    }

    [RTTI.Serializable(0x31F37D84AC388C63, GameType.DS)]
    public enum EDSPatrolPathCoverType : int8
    {
        None = 0,
        CoverLeft = 1,
        CoverRight = 2,
        StandCoverLeft = 3,
        StandCoverRight = 4,
        CoverUnderFire = 5,
    }

    [RTTI.Serializable(0x16AADED5D750DFDB, GameType.DS)]
    public enum EDSPatrolPathType : int8
    {
        None = 0,
        Mule = 1,
        MuleVehicle = 2,
        Warrior = 3,
        Cliff = 4,
    }

    [RTTI.Serializable(0x1290FCFAC956B6CB, GameType.DS)]
    public enum EDSPersonGender : int8
    {
        Unset = 0,
        Man = 1,
        Female = 2,
    }

    [RTTI.Serializable(0x8A7E0BFAED41937C, GameType.DS)]
    public enum EDSPlaySentenceNodePriority : int8
    {
        RobotVoiceHigh = 0,
        RobotVoiceMiddle = 1,
        RobotVoiceLow = 2,
    }

    [RTTI.Serializable(0x16CC5E3FAB40EBFD, GameType.DS)]
    public enum EDSPlayerActionFlagExported : int32
    {
        NONE = 0,
        STOP = 1,
        SUBJECT_CAMERA = 2,
        TPS_CAMERA = 3,
        CAMERA_ZOOM = 4,
        ENABLE_CHANGE_WEAPON = 5,
        DISABLE_CHANGE_WEAPON = 6,
        ENABLE_CHANGE_ITEM = 7,
        DISABLE_CHANGE_ITEM = 8,
        ENABLE_CHANGE_SUITPARTS = 9,
        DISABLE_CHANGE_SUITPARTS = 10,
        ENABLE_CHANGE_EQUIPMENT = 11,
        DISABLE_CHANGE_EQUIPMENT = 12,
        DISABLE_DANDLE_BB = 13,
        DISABLE_CHANGE_SHOES = 14,
        DISABLE_CHANGE_BAGGAGE_PLACE = 15,
        ENABLE_CHANGE_MAGAZINE = 16,
        DISABLE_CHANGE_MAGAZINE = 17,
        DISABLE_CHANGE_ALL = 19,
        PALALLEL_SHIFT = 20,
        COMBO_MISS = 21,
        COMBO_ATTACKING = 22,
        COMBO_ATTACK_STATE = 23,
        COMBO_STATE = 24,
        CAN_USE_FAKE_HOLO = 25,
        USING_FAKE_HOLO = 26,
        CAN_USE_FAKE_HOLO_ACTION = 27,
        VEHICLE_DRIVING = 28,
        CART_DRIVING = 29,
        ENABLE_SUBJECT_CAMERA = 30,
        DISABLE_SUBJECT_CAMERA = 31,
        DISABLE_RESET_CAMERA = 32,
        DISABLE_FALL = 33,
        DISABLE_FALL_ENABLE_CLIFF = 34,
        DISABLE_DASH = 35,
        DISABLE_GUN_HOLD = 36,
        DISABLE_IMPACT_EFF_SLIDE = 37,
        DISABLE_IMPACT_EFF_TRIG = 38,
        USE_SEQ_MOTION = 40,
        USE_BASIC_MOTION = 41,
        INCITE_TAKE_A_BREAK = 42,
        FORCE_BASIC_SLOPE_IS_PLANE = 43,
        FORCE_SLOPE_IS_PLANE = 44,
        USE_MOVE_SLOPE_FRONT_IS_STICK = 45,
        TURN = 46,
        TOUCH_WALL = 47,
        HIGGS_PHASE3_MODE = 48,
        ROPE_MOVE_PLANE_BACK = 49,
        ROPE_MOVE_CLIMB_UP = 50,
        ECO_MOTION = 51,
        ECO_MOTION_PLAY_VOICE = 52,
        UpperLookMission = 53,
        USE_FORCE_BRAKE_MOVE = 54,
        HUGE_CATCHER_ABSORB_GUN_ATTACK_TIME = 55,
        CLIFF_MODE = 56,
        HUGE_ABSORB_FAILD_TO_FALL = 57,
        VIEW_CLIFF_EDGE = 58,
        AUTO_STEP_DOWN = 59,
        AUTO_STEP_ON = 60,
        FALL_CHECK_IS_LONG = 61,
        CLIFF_IS_NONE_SAFE_ACTION_AREA = 62,
        CART_IS_DRAG_BAT_NOT_ATTACH = 63,
        FOOT_IMPACT_IS_ONESELF_MODE = 64,
        FOOT_IMPACT_IS_SLIDE_MODE = 65,
        BREATH_OUT_DOWN = 66,
        PRE_TIRE_IDLE = 67,
        HUNTER_DRAG_TO_BACK = 68,
        IN_AIR = 69,
        FALLING = 70,
        EVASION = 71,
        TAKE_A_BREAK_INVISIBLE_BAGGAGE = 73,
        WAIT_DEATH_FROM_DAMAGE = 74,
        ATTACK_MODE = 75,
        AIM_MODE = 76,
        AIM_TURN = 78,
        USE_UPPER_AIM_MOTION = 79,
        CAN_CHANGE_HOLD_GUN_MODE = 80,
        RELOAD_READY = 81,
        RELOAD_MODE = 82,
        UNHOLD_GUN_AIM_MODE = 83,
        ENABLE_KEEP_AIM_POSE = 84,
        CAN_FIRE_ACTION = 85,
        FIRING = 86,
        CAN_EVADE_TO_FAST_ATTACK = 87,
        TAKE_A_BREAK = 88,
        TAKE_A_BREAK_SLEEPING_STATE = 89,
        TAKE_A_BREAK_SLEEPING = 90,
        TAKE_A_BREAK_NAP = 91,
        TAKE_A_BREAK_IS_CROUCH = 92,
        TAKE_A_BREAK_IS_SITDOWN = 93,
        OKAN_MOTION = 94,
        GUARD_MODE = 95,
        GUARD_MODE_RIGHT = 96,
        GUARD_MODE_LEFT = 97,
        GUARD_MODE_INTERRUPTION_READY = 98,
        GUARD_MODE_INTERRUPTION = 99,
        GUARD_MODE_RIGHT_ACTIVE = 100,
        GUARD_MODE_LEFT_ACTIVE = 101,
        GUARD_MODE_RIGHT_ACTIVE_HOLDED = 102,
        GUARD_MODE_LEFT_ACTIVE_HOLDED = 103,
        TRY_GUARD_MODE = 104,
        TRY_GUARD_IS_LEFT = 105,
        TRY_GUARD_IS_RIGHT = 106,
        TRY_GUARD_IS_MOST_SAFETY = 107,
        REQUEST_GUARD_MODE_LEFT_ICON = 108,
        REQUEST_GUARD_MODE_RIGHT_ICON = 109,
        REQUEST_GUARD_MODE_LEFT_ICON_SHOW_ONLY = 110,
        REQUEST_GUARD_MODE_RIGHT_ICON_SHOW_ONLY = 111,
        REQUEST_GUARD_MODE_ICON_IS_STOP = 112,
        STOP_BREATH_LEFT = 113,
        STOP_BREATH_RIGHT = 114,
        STOP_BREATH_LEFT_ACTIVE = 115,
        STOP_BREATH_RIGHT_ACTIVE = 116,
        STOP_BREATH_LEFT_HAND_LOWER = 117,
        STOP_BREATH_HARD = 118,
        CRYPTO_EATING_LEFT = 119,
        CRYPTO_EATING_RIGHT = 120,
        CRYPTO_GETING_LEFT = 121,
        CRYPTO_GETING_RIGHT = 122,
        WAKED_HAND_CUFF_TERMINAL = 123,
        WAKED_HAND_CUFF_TERMINAL_KEEP_ACTION = 124,
        WAKED_HAND_CUFF_TERMINAL_KEEP_ACTION_WAKE_FRAME = 125,
        HAND_CUFF_TERMINAL_USE_MOTION = 126,
        NO_TARMINAL_START = 127,
        CAN_TARMINAL_START = 128,
        CAN_TARMINAL_START_AND_MOTION = 129,
        CAN_TARMINAL_START_AND_NO_MOTION = 130,
        CAN_TARMINAL_START_AND_ZIPLINE = 131,
        __0 = 132,
        CAN_TARMINAL_START_AND_DIRECT_CARGO = 133,
        NO_NATURAL_PUBLIC_MOTION = 134,
        TO_GLOVE_TOUCH_EFFECT = 135,
        CAN_LOADING_ACTION_SEAQUENCE_ONCE = 137,
        MOVE_HEADING_TO_TURN_HEADING = 138,
        DANGER_SLOPE_MOVE = 139,
        JUDGE_CATCH_CATCH_BAGGAGE_KEEP_ACTION = 140,
        CAN_CATCH_BAGGAGE_KEEP_ACTION = 141,
        CATCH_BAGGAGE_KEEP_MODE = 142,
        CAN_GUARD_MODE_KEEP_ACTION = 143,
        ENABLE_DASH_CAMERA_SHAKE = 145,
        SPRINT_TO_SLIDING = 146,
        PICKUP_COLLECTIBLE_RIGHT = 147,
        PICKUP_COLLECTIBLE_LEFT = 148,
        PICKUP_READY_BAGGAGE_RIGHT = 149,
        PICKUP_READY_BAGGAGE_LEFT = 150,
        PICKUP_BAGGAGE_RIGHT = 151,
        PICKUP_BAGGAGE_LEFT = 152,
        PICKUP_BAGGAGE_RIGHT_FROM_DELEVERY = 153,
        PICKUP_BAGGAGE_LEFT_FROM_DELEVERY = 154,
        PUT_BAGGAGE_RIGHT = 157,
        PUT_BAGGAGE_LEFT = 158,
        CATCH_BAGGAGE_RIGHT = 159,
        CATCH_BAGGAGE_LEFT = 160,
        PICK_FROM_BACKPACK_RIGHT = 161,
        PICK_FROM_BACKPACK_LEFT = 162,
        SWAP_BAGGAGE = 163,
        FOLD_BAGGAGE_RIGHT = 164,
        FOLD_BAGGAGE_LEFT = 165,
        PACK_BAGGAGE_RIGHT = 166,
        PACK_BAGGAGE_LEFT = 167,
        SWING_BAGGAGE_RIGHT = 168,
        SWING_BAGGAGE_LEFT = 169,
        REQUEST_BAGGAGE_FOLD = 171,
        LOWER_HACK = 172,
        SNS_PLAY = 173,
        HUNTER_SWINGING_L = 175,
        HUNTER_SWINGING_R = 176,
        HUNTER_SWINGING_ATTACKING = 177,
        BELL_DOWN = 179,
        BB_HOLD = 180,
        DOWN_TO_GETUP = 181,
        HIP_DOWN = 182,
        HIP_DOWN_TO_GETUP = 183,
        UNCONSCIOUS_START = 184,
        UNCONSCIOUS_LOOP = 185,
        RIVER_DRIVING = 186,
        RIVER_DRIVING_AND_START = 187,
        WATER_CHECK_FROM_GROUND = 188,
        WATER_CHECK_FROM_HIT_GROUND = 189,
        JUMP_UP = 190,
        JUMP_DOWN = 191,
        LAND_STEP = 192,
        SLIDING_MOTION = 193,
        SLIDING_ROLLING_MOTION = 194,
        SLIDING_STAND_KEEP_MOTION = 195,
        SLIDING_CATCH_GROUND = 196,
        FOOT_SLIP_MOTION = 197,
        DASH_START = 198,
        SLIPPING = 199,
        SLIPPING_RECOVER_SUCCESS = 200,
        UNSTABLE_BALANCE = 201,
        UNSAFE_ACTION = 202,
        LITTLE_SLIP_FALL = 203,
        FOOT_HEAVY_GRAVITY = 204,
        DRAGED_BY_GAZER = 205,
        DRAGGED_BY_GAZER_CAMERA = 206,
        NO_DAMAGE = 207,
        NO_DAMAGE_HIT = 208,
        NEVER_DEAD = 209,
        USING_ROPE = 210,
        USE_SUBJECTIVE_LOOK_IK = 211,
        DODGE_TO_KEEP_DASH = 213,
        RIDE_VEHICLE_HAS_BAGGAGE = 214,
        RIDE_VEHICLE_HAS_BAGGAGE_WITH_DELIVERY_TAG = 215,
        DISABLE_LUNG_RECOVERY = 216,
        DISABLE_CONSCIOUS_RECOVERY = 217,
        BC_STRESS_FULL = 219,
        BC_SEE_GAZER = 220,
        ENABLE_BREATH_HOLD = 221,
        DECREASE_SHOES_DURABILITY = 222,
        DECREASE_STAMINA_BY_MOVING = 223,
        CAN_USE_BAGGAGE_SENSOR = 224,
        CAN_USE_SUITPARTS = 225,
        ODRADEK_CAN_PLAY_ENCOUNTER_ACT = 226,
        FORCE_AIM_MODE = 227,
        DISP_SEND_LIKE_ICON = 228,
        CANNOT_SEND_LIKE = 229,
        DISABLE_PAUSE_MENU = 230,
        VOLATILE_START = 234,
        ENABLE_BACK_INPUT_ONCE = 235,
        FORCE_BACK_INPUT_ONCE = 236,
        MOT_SPEED_COPY_FROM_STOP_SPEED = 237,
        BACK_INPUT_NEVER_STOPOUT_ONCE = 238,
        ADJUST_TO_MOVE_FRAME = 239,
        NO_DAMAGE_ONCE = 240,
        NO_DAMAGE_HIT_ONCE = 241,
        NEVER_DEAD_ONCE = 242,
        DEATH_IS_RAGDOLL_FROM_DAMAGE_ONCE = 243,
        USE_BASIC_HAND_L_IK_ONCE = 246,
        USE_BASIC_HAND_R_IK_ONCE = 247,
        DISABLE_CHANGE_ALL_ONCE = 248,
        DISABLE_UPDATE_MISSION_BAGGAGE_INFOLOG = 249,
        LOWER_HACK_ONCE = 250,
        DUMMY = 251,
        VOLATILE_END = 252,
        FLAG_COUNT = 253,
    }

    [RTTI.Serializable(0x426413B4BF536F01, GameType.DS)]
    public enum EDSPlayerActionStateExported : int32
    {
        None = 0,
        Basic = 1,
        LowerHack = 2,
        Dodge = 3,
        Pickup = 4,
        CareForBB = 5,
        TakeABreak = 6,
        Interactive = 7,
        Climb = 8,
        RopeMove = 9,
        Natural = 10,
        Jump = 11,
        RideVehicle = 12,
        Zipline = 13,
        FastAttack = 14,
        Fall = 15,
        StepOn = 16,
        Elude = 17,
        Damage = 18,
        InGameGesture = 19,
        Ghost = 20,
        HiggsPhase = 21,
        Death = 22,
        Sequence = 23,
        PrivateRoom = 24,
        HeartmanRoom = 25,
    }

    [RTTI.Serializable(0x3E247C3F86351483, GameType.DS)]
    public enum EDSPlayerCameraShakeTypeExported : int32
    {
        CliffSlide = 7,
        GazerDrag = 11,
        CartCodeBreak = 1,
        FootStepHeavy = 0,
        PreGust = 5,
        Gust = 6,
        SlipInWater = 8,
        DetectGazer = 9,
        NearGazer = 10,
        ViewCliffEdge = 2,
        DetectGazerEnd = 12,
        SlipDown = 3,
        WallPush = 4,
        OdradekSpin = 13,
        OdradekSurprise = 14,
        SlopeDownMove = 15,
        SlipStart = 16,
        Dash = 17,
        SlipSlideL = 18,
        SlipSlideR = 19,
        LittleSlip = 20,
        JumpLanded = 21,
        FallLanded = 22,
        __0 = 23,
        __1 = 24,
        __2 = 25,
        StickyPullBaggageS = 26,
        StickyPullBaggageL = 27,
        ZiplineMove = 28,
        ZiplineMoveDemo = 29,
        HiggsTarryPlayerPunch = 30,
        HiggsTarryHiggsPunch = 31,
        HiggsTarryDefault = 32,
        PartsEquip = 33,
        HighSpeedRiverDrive = 34,
        HighSpeedMove = 35,
        VehicleTackle = 36,
        Max = 37,
    }

    [RTTI.Serializable(0xFBA2282CBED6E02E, GameType.DS)]
    public enum EDSPlayerDamageReactionType : int8
    {
        None = 0,
        Flinch = 1,
        Small = 2,
        Landed = 3,
        SlipDown = 4,
        BlastWave = 5,
        Stagger = 6,
        StaggerLong = 7,
        Down = 8,
        Blow = 9,
        BlowStrong = 10,
        ByImpact = 11,
    }

    [RTTI.Serializable(0xA4BE97DB3F7E5952, GameType.DS)]
    public enum EDSPlayerFatigueLevelExported : int8
    {
        Level1 = 0,
        Level2 = 1,
        Level3 = 2,
        Level4 = 3,
        Level5 = 4,
        Invalid = 5,
    }

    [RTTI.Serializable(0xD874B8B18381DD7A, GameType.DS)]
    public enum EDSPlayerGDVoicePriority : int32
    {
        Low = 0,
        High = 1,
    }

    [RTTI.Serializable(0xA549CD0CE4577C95, GameType.DS)]
    public enum EDSPlayerGroundEffectType : int8
    {
        Soil = 0,
        Blood = 1,
    }

    [RTTI.Serializable(0x9D341185B02230D8, GameType.DS)]
    public enum EDSPlayerMesh : int8
    {
        Root = 0,
        Body = 1,
        Head = 2,
        Arm = 3,
        HoodOn = 4,
        HoodOff = 5,
        HoodSwitch = 6,
        BootsL = 7,
        BootsR = 13,
        BootsBreakL = 8,
        BootsBreakR = 14,
        BootsRepairedL = 9,
        BootsRepairedR = 15,
        BootsSoleL = 10,
        BootsSoleR = 16,
        SoleGrassL = 11,
        SoleGrassR = 17,
        BareFootL = 12,
        BareFootR = 18,
        PrivateFootNakedFingerL = 19,
        PrivateFootNakedFingerBloodL = 21,
        PrivateFootNakedFingerBloodNailL = 22,
        PrivateFootNakedFingerR = 23,
        PrivateFootNakedFingerBloodR = 25,
        PrivateFootNakedFingerBloodNailR = 26,
        HairDef = 28,
        HairHoodOn = 29,
        BBPod = 30,
        BBPodPipe = 31,
        ExoMain = 32,
        ExoSpeed = 32,
        BodyLamp = 33,
        ArmLamp = 34,
        Balloon = 35,
        Misanga = 36,
        HairCap = 37,
        ExoSub = 38,
        Speed = 39,
        Pile = 40,
        BatteryEmission = 41,
        Heavy = 42,
        EmissionHeavy = 43,
        LegIn = 44,
        LegOut = 45,
        LegInLamp = 46,
        LegOutLamp = 47,
        HairHoodOnGlassesOn = 48,
        HairHoodOffGlassesOn = 49,
        HairWig = 50,
        SubPouch = 51,
        DreamCatcher = 52,
        Glove = 53,
        GlovePower = 54,
        HoodswitchLamp = 55,
        HoodonLamp = 56,
        HoodoffLamp = 57,
        EarDef = 58,
        EarDam = 59,
        OxiShieldOn = 60,
        OxiShieldOff = 61,
        OxiShieldSwitch = 62,
        BackpackBelt = 63,
        HoodHiggs = 64,
        WearPorter = 65,
        NeckwearPorter = 66,
        WearAcro = 67,
        Neckwear = 68,
        Strand = 69,
        BeachLegInLamp = 70,
        BeachLegOutLamp = 71,
        Hip = 72,
        HipGun = 73,
        HairPonytail = 74,
        __0 = 75,
        __1 = 76,
        __2 = 77,
        GravityGlove = 78,
        OxiMechOn = 81,
        OxiMechOff = 82,
        OxiMechswitch = 83,
        StaminaLamp01 = 84,
        StaminaLamp02 = 85,
        Boots_repealed_ = 87,
        BootsBreak_repealed_ = 88,
        SoleGrass_repealed_ = 89,
        BareFoot_repealed_ = 90,
        Hair = 91,
        HairSuitMule = 92,
        HoodOtterOn = 93,
        HoodOtterOff = 94,
    }

    [RTTI.Serializable(0xA39352BF63379002, GameType.DS)]
    public enum EDSPlayerMissionMessageEventType : int8
    {
        Invalid = 0,
        BaggageWeightReached = 1,
        AmmoIsLess = 2,
    }

    [RTTI.Serializable(0xAF0686C2BD3EF422, GameType.DS)]
    public enum EDSPlayerMoveTypeExported : int32
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Dash = 3,
    }

    [RTTI.Serializable(0x60FC5D164D777DCC, GameType.DS)]
    public enum EDSPlayerMusicKind : int8
    {
        None = 0,
        Whistle = 1,
        Harmonica = 2,
    }

    [RTTI.Serializable(0xF5E3C277675486DF, GameType.DS)]
    public enum EDSPlayerRumbleTypeExported : int32
    {
        HeavyFootStep = 0,
        PreGust = 6,
        Gust = 7,
        SlipInWater = 8,
        BalanceOutLeft = 1,
        BalanceOutRight = 2,
        CatchKeep = 3,
        SlipDown = 4,
        WallPush = 5,
        Grab = 9,
        SlipStart = 10,
        SlipSlideL = 11,
        SlipSlideR = 12,
        LittleSlip = 13,
        JumpLanded = 14,
        FallLanded = 15,
        CatchBaggage = 16,
        GrabRopeClimbing = 17,
        HiggsTarryPlayerPunch = 18,
        HiggsTarryHiggsPunch = 19,
        __0 = 20,
        __1 = 21,
        __2 = 22,
        __3 = 23,
        Flinch = 24,
        IsMarkedByMule = 25,
        Max = 26,
    }

    [RTTI.Serializable(0x1B333C67F81D73F0, GameType.DS)]
    public enum EDSPlayerShoesType : int8
    {
        BareFoot = 0,
        ShoesA_Unused = 1,
        SoleGrass = 2,
        NormalBoots = 3,
        StableBootsLv1 = 4,
        StableBootsLv2 = 5,
        StableBootsLv3 = 6,
        MuleBoots = 7,
        TerroristBoots = 8,
        ChiralBoots = 9,
    }

    [RTTI.Serializable(0x107972457DE2A2FB, GameType.DS)]
    public enum EDSPlayerStanceExported : int32
    {
        Stand = 0,
        Crouch = 1,
    }

    [RTTI.Serializable(0xFF0573DA6E02C9B8, GameType.DS)]
    public enum EDSPlayerTakeABreakType : int8
    {
        SittingFloor = 0,
        Crouch = 1,
    }

    [RTTI.Serializable(0xC1A91F5C3829CD56, GameType.DS)]
    public enum EDSPlayerTracePointType : int8
    {
        Dead = 0,
        Rest = 1,
    }

    [RTTI.Serializable(0xE069D0F77471F15D, GameType.DS)]
    public enum EDSPlayerVoiceSituationType : int32
    {
        DroppedBaggages = 0,
        LifeRateIs40PerOrLess = 1,
        StaminaRateIs30PerOrLess = 2,
        FewAmmo = 3,
        InRain = 4,
        StaminaRateIs50PerOrLess = 5,
        DiscoverSomething = 6,
        Humming = 7,
    }

    [RTTI.Serializable(0x3AA9926CA6A4365, GameType.DS)]
    public enum EDSPrivateRoomEventType : int8
    {
        ExitRoom = 0,
        DirectExitRoom = 1,
        UseShower = 2,
        ShowEquipmentShelf = 3,
        UseBBTool = 4,
        FastTravel = 5,
        EnterRoom = 6,
        UseWC = 7,
        ToUrinate = 8,
        ToFeces = 9,
        UseWashBasin = 10,
        ShowSuit = 11,
        CustomizeBackpack = 12,
        UseTerminal = 13,
        ShowTable = 14,
        WatchFigure = 15,
        ShowDreamCatcher = 16,
        RemoveCap = 17,
        RemoveGlasses = 18,
        LookLeft = 19,
        LookRight = 20,
        GestureThumbsUp = 21,
        GesturePleaseLook = 22,
        GestureStepping = 23,
        Look_R90 = 24,
        Look_R45 = 25,
        Look_0 = 26,
        Look_L45 = 27,
        Look_L90 = 28,
        Watch_Map = 31,
        Watch_Entrance = 32,
        Watch_Suit = 33,
        Watch_Weapon = 34,
        Watch_Shower = 35,
        Watch_Washroom = 36,
        Leving = 37,
        Groin = 38,
        Bust = 39,
        EndPreparationMenu = 40,
        EndFastTravel = 41,
        EndTerminal = 42,
        EndCustomizeBackpack = 43,
        FromShowCaseAtoB = 44,
        FromShowCaseBtoA = 45,
        FromShowCaseBtoC = 46,
        FromShowCaseCtoB = 47,
        FromShowCaseCtoD = 48,
        FromShowCaseDtoC = 49,
        FromShowCaseDtoE = 50,
        FromShowCaseEtoD = 51,
        FromShowCaseAtoBed = 52,
        FromShowCaseBtoBed = 53,
        FromShowCaseCtoBed = 54,
        FromShowCaseDtoBed = 55,
        FromShowCaseEtoBed = 56,
        FromTableToBed = 57,
    }

    [RTTI.Serializable(0xCD5F4009B7359CD3, GameType.DS)]
    public enum EDSPrivateRoomIconType : int8
    {
        Circle = 0,
        Cross = 1,
        Triangle = 2,
        Square = 3,
        None = 4,
    }

    [RTTI.Serializable(0x6EA1F35B458DAC62, GameType.DS)]
    public enum EDSPrivateRoomMode : int8
    {
        Default = 0,
        Table = 1,
        ShowCaseA = 2,
        ShowCaseB = 3,
        ShowCaseC = 4,
        FigureShelf = 5,
        ShowCaseD = 6,
        ShowCaseE = 7,
    }

    [RTTI.Serializable(0xEDD01365328BD66D, GameType.DS)]
    public enum EDSPrivateRoomObjectConditionType : int8
    {
        Always = 0,
        Has = 1,
    }

    [RTTI.Serializable(0xE87D5551C1D3DD2C, GameType.DS)]
    public enum EDSPrivateRoomObjectType : int8
    {
        Entity = 0,
        Weapon = 1,
        Item = 2,
        SuitParts = 3,
        WeaponWithFact = 4,
    }

    [RTTI.Serializable(0xC1495AC426746E3D, GameType.DS)]
    public enum EDSProjectileId : int8
    {
        None = 0,
        AssaultRifle = 1,
        AssaultRifleLv2 = 2,
        AssaultRifleLv3 = 3,
        AssaultRifleLv4 = 4,
        AssaultRifleBloodBullet = 5,
        AssaultRifleLv2BloodBullet = 6,
        AssaultRifleLv3BloodBullet = 7,
        AssaultRifleLv4BloodBullet = 8,
        AssaultRifleGoldenBullet = 9,
        AssaultRifleLv2GoldenBullet = 10,
        AssaultRifleLv3GoldenBullet = 11,
        AssaultRifleLv4GoldenBullet = 12,
        Grenade = 13,
        BloodGrenade = 14,
        BloodGrenadeLv1Extend = 15,
        BloodGrenadeLv2 = 16,
        ElectricalGrenadeLv1 = 17,
        ElectricalGrenadeLv2 = 18,
        ElectricalGrenadePlace = 19,
        CoatingSpray = 20,
        SmokeGrenade = 21,
        SmokeGrenadeLv2 = 22,
        FreezeGrenade = 23,
        TranquilizerGun = 24,
        AmnioticFluidGrenade = 25,
        ExGrenade0 = 26,
        ExGrenade1 = 27,
        ExGrenade1Plus = 28,
        ExGrenade2 = 29,
        BolaGun = 30,
        BolaGunLv2 = 31,
        ShotGun = 32,
        ShotGunLv2 = 33,
        ShotGunLv3 = 34,
        ShotGunBloodBullet = 35,
        ShotGunBloodBulletLv2 = 36,
        ShotGunBloodBulletLv3 = 37,
        ShotGunGoldenBulletLv3 = 38,
        HandGun = 39,
        HandGunLv2 = 40,
        HandGunLv3 = 41,
        HandGunBloodBullet = 42,
        HandGunBloodBulletLv2 = 43,
        HandGunBloodBulletLv3 = 44,
        HandGunGoldenBullet = 45,
        HandGunGoldenBulletLv2 = 46,
        HandGunGoldenBulletLv3 = 47,
        BloodHandGun = 48,
        BloodHandGunLv2 = 49,
        AmelieHandGun = 50,
        C4 = 51,
        GazerBalloon = 52,
        MultiRod = 53,
        SamBall = 54,
        SamBallLv2 = 55,
        GrenadeShell = 56,
        BloodGrenadeShell = 57,
        SlipGrenadeShell = 58,
        ElectricalGrenadeShell = 59,
        AmnioticFluidGrenadeShell = 60,
        TranquilizerGrenadeShell = 61,
        AssaultRifleRubberBullet = 62,
        AssaultRifleRubberBulletLv2 = 63,
        AssaultRifleRubberBulletLv3 = 64,
        AssaultRifleRubberBulletLv4 = 65,
        AssaultRifleRubberBloodBullet = 66,
        AssaultRifleRubberBloodBulletLv2 = 67,
        AssaultRifleRubberBloodBulletLv3 = 68,
        AssaultRifleRubberBloodBulletLv4 = 69,
        AssaultRifleRubberGoldenBulletLv3 = 70,
        AssaultRifleRubberGoldenBulletLv4 = 71,
        ShotGunRubberBullet = 72,
        ShotGunRubberBulletLv2 = 73,
        ShotGunRubberBulletLv3 = 74,
        ShotGunRubberBloodBullet = 75,
        ShotGunRubberBloodBulletLv2 = 76,
        ShotGunRubberBloodBulletLv3 = 77,
        ShotGunRubberGoldenBulletLv3 = 78,
        StickyBullet = 79,
        FourConsecutiveMissile = 80,
        FourConsecutiveMissileBlood = 81,
        SpreadMissile = 82,
        SpreadMissileBlood = 83,
        SpreadMissileChild = 84,
        SpreadMissileChildBlood = 85,
        EnemyAssaultRifle = 86,
        EnemyAssaultRifleRubberBullet = 87,
        HiggsAssaultRifleBullet = 88,
        Ww1Rifle = 89,
        Ww1ShotGun = 90,
        Ww1Grenade = 91,
        Ww1MachineGun = 92,
        Ww2SubmachineGun = 93,
        Ww2Rifle = 94,
        Ww2Missile = 95,
        Ww2MissileType2 = 96,
        Ww2SmokeGrenade = 97,
        VietnamAssault = 98,
        VietnamAssaultWithGrenader = 99,
        VietnamAssaultWithGrenaderShell = 100,
        VietnamMachinegun = 101,
        VietnamGrenade = 102,
        CliffRifle = 103,
        AfghanRifle = 104,
        HiggsKnife = 105,
        DemensAssaultRifleBullet = 106,
        DemensShotGunBullet = 107,
        EnemyGrenade = 108,
        Ww2Grenade = 109,
        AfghanGrenade = 110,
        PoisonGasShell = 111,
        __1 = 112,
        Ww2AirPlaneMachinegun = 113,
        Ww2HeavyMachinegun = 114,
        DemensElectricalGrenade = 115,
        __2 = 116,
    }

    [RTTI.Serializable(0x8664CF834C8F1561, GameType.DS)]
    public enum EDSQpidBandWidthLevel : int8
    {
        BandWidthLevel0 = 0,
        BandWidthLevel1 = 1,
        BandWidthLevel2 = 2,
        BandWidthLevel3 = 3,
        BandWidthLevel4 = 4,
        BandWidthLevel5 = 5,
    }

    [RTTI.Serializable(0x1524F816E18636BB, GameType.DS)]
    public enum EDSQpidLevelUpEvent : int8
    {
        None = 0,
        Briddges = 1,
        UCA = 2,
        BriddgesAndUCA = 3,
    }

    [RTTI.Serializable(0x24689E65406FEFE9, GameType.DS)]
    public enum EDSQpidUnitDefaultGroup : int8
    {
        QpidUnitDefaultGroupNone = 0,
        QpidUnitDefaultGroupFragile = 1,
        QpidUnitDefaultGroupBridges = 2,
    }

    [RTTI.Serializable(0xAD017DC1065A79E4, GameType.DS)]
    public enum EDSQpidUnitState : int8
    {
        QpidUnitStateUndiscovered = 0,
        QpidUnitStateUnconnected = 1,
        QpidUnitStateJoinedBridges = 2,
        QpidUnitStateJoinedUCA = 3,
    }

    [RTTI.Serializable(0xF2F150A4D2704911, GameType.DS)]
    public enum EDSRadioCollisionGroup : int8
    {
        Radio = 0,
        Robo = 1,
        Deadman500 = 2,
        Default = 3,
    }

    [RTTI.Serializable(0x31BBC28D57709774, GameType.DS)]
    public enum EDSRadioMetadataClassification : int8
    {
        None = 0,
        BridgingSound = 1,
        StartingSound = 2,
        SquelchOpeningSound = 3,
        Sentence = 4,
        SquelchClosingSound = 5,
        EndingSound = 6,
    }

    [RTTI.Serializable(0xD72D79166581187, GameType.DS)]
    public enum EDSRadioPlaybackEvent : int8
    {
        Started = 0,
        Completed = 1,
        Aborted = 2,
    }

    [RTTI.Serializable(0xF678F672637063F7, GameType.DS)]
    public enum EDSRadioPlaybackMode : int8
    {
        Sequential = 0,
        Random = 1,
    }

    [RTTI.Serializable(0xFBB67686BE498337, GameType.DS)]
    public enum EDSRadioPriority : int8
    {
        Low = 0,
        BelowNormal = 1,
        Normal = 2,
        AboveNormal = 3,
        High = 4,
        Higher = 5,
        Highest = 6,
    }

    [RTTI.Serializable(0xB9F3F9CA802D5E4, GameType.DS)]
    public enum EDSRadioRepeatMode : int8
    {
        DoNotRepeat = 0,
        RepeatAll = 1,
        RepeatLastOne = 2,
    }

    [RTTI.Serializable(0x2D8146D8E48E6F78, GameType.DS)]
    public enum EDSRadioRestartFrom : int8
    {
        None = 0,
        BeginningOfSentence = 1,
        BeginningOfScript = 2,
    }

    [RTTI.Serializable(0x4B6D4A51EAA6A1FE, GameType.DS)]
    public enum EDSRadioStartFrom : int8
    {
        StartingSound = 0,
        SquelchOpeningSound = 1,
        Sentence = 2,
    }

    [RTTI.Serializable(0xC9502EF0C3CE8F87, GameType.DS)]
    public enum EDSRadioTransition : int8
    {
        Immediately = 0,
        AtTheEndOfSentence = 1,
        AtTheEndOfSentenceSequence = 2,
    }

    [RTTI.Serializable(0x27DF607120932879, GameType.DS)]
    public enum EDSRadioTriggerMode : int8
    {
        Single = 0,
        Multiple = 1,
    }

    [RTTI.Serializable(0x87E3B887053359D1, GameType.DS)]
    public enum EDSReliefSupplySettings_Type : int16
    {
        BloodPack = 0,
        Weapon = 1,
        WeaponGrenade = 2,
        WeaponLauncher = 3,
    }

    [RTTI.Serializable(0x9FE4A9AA5C6B5024, GameType.DS)]
    public enum EDSRestingPlaceType : int8
    {
        Default = 0,
    }

    [RTTI.Serializable(0xF0E4EEBE7CB81915, GameType.DS)]
    public enum EDSReticleType : int8
    {
        None = 0,
        Gun = 1,
        Shotgun = 2,
        Throw = 3,
        Bola = 4,
        Sticky = 5,
        LockOnMissile = 6,
        CoatingSpray = 7,
    }

    [RTTI.Serializable(0xB3975EB377C47967, GameType.DS)]
    public enum EDSRewardRank : int8
    {
        None = 0,
        Rank_E = 0,
        Rank_D = 1,
        Rank_C = 2,
        Rank_B = 3,
        Rank_A = 4,
        Rank_S = 5,
        Rank_SS = 6,
        Rank_SSS = 7,
        Rank_SSSS = 8,
    }

    [RTTI.Serializable(0xF497580C27DD8467, GameType.DS)]
    public enum EDSRicochetType : int8
    {
        None = 0,
        Default = 1,
        Blood = 2,
        Gold = 3,
        Rubber = 4,
        RubberBlood = 5,
        RubberGold = 6,
        CapsuleBlood = 7,
        SlipGrenade = 8,
        ElectricSpear = 9,
        Middle = 10,
        Large = 11,
        CapsuleBloodMiddle = 12,
        CapsuleBloodLarge = 13,
        AirPlaneMachinegun = 14,
        HeavyMachinegun = 15,
    }

    [RTTI.Serializable(0x9697A693BBC5F89D, GameType.DS)]
    public enum EDSRoadJunctionConnectionType : int8
    {
        None = 0,
        OpenOnly = 1,
        AttachToRoad = 2,
    }

    [RTTI.Serializable(0xFC137BFD93AD0E1B, GameType.DS)]
    public enum EDSRouteConnectionType : int32
    {
        Normal = 0,
        Caution = 1,
    }

    [RTTI.Serializable(0x215E8E53B40F7C4E, GameType.DS)]
    public enum EDSRuledNameCollisionTriggerType : int8
    {
        None = 0,
        TarSwamp = 1,
        M00270_DestinationTarSwamp = 2,
        DeliveryTerminal = 3,
        AutoTakeBreak = 4,
        GreatView = 5,
        Onsen = 6,
        GeneralEvent = 7,
        GeneralFocusEvent = 8,
        HalationCheck = 9,
        TarFastMove = 10,
        AmelieDemo_TarSwamp = 11,
        WaterIsShallow = 12,
        ReturnNG = 13,
        ReturnOK = 14,
        WaterfallDeathArea = 15,
        PlayerAction = 16,
        RopeMoveDownStop = 17,
        CameraSmoke = 18,
        DisableCreateRestMark = 19,
        EnableDelivery = 20,
        RainShelterSkipArea = 21,
        FallDeathStartSelectArea = 22,
        BossCatcher = 23,
        DisableSignboard = 24,
    }

    [RTTI.Serializable(0xFB16FE3D1A81C4BF, GameType.DS)]
    public enum EDSSaveOption : int16
    {
        None = 0,
        AllowInsideBTArea = 1,
        AllowInsideMuleArea = 2,
        AllowInAnyGameState = 4,
        WaitInAnyGameState = 8,
        WaitFade = 16,
    }

    [RTTI.Serializable(0x201BF6AC70CC1970, GameType.DS)]
    public enum EDSSceneType : int8
    {
        Default = 0,
        GlobalForMission = 1,
        GlobalForFreeMission = 2,
        GlobalForNormalScene = 3,
        MissionScene = 4,
        FreeMissionScene = 5,
        NormalSceneForMissionSceneEvent = 6,
    }

    [RTTI.Serializable(0xEDE48F7664D52C94, GameType.DS)]
    public enum EDSSendsLikeRecipientType : int32
    {
        ToNpc = 0,
        ToOnlineUser = 1,
    }

    [RTTI.Serializable(0xC030BE8EE27B4106, GameType.DS)]
    public enum EDSSharedGimmickAreaType : int8
    {
        Global = 0,
        Tile = 1,
    }

    [RTTI.Serializable(0x3C0FD0EDC8F622BF, GameType.DS)]
    public enum EDSSharedGimmickRealizeType : int8
    {
        None = 0,
        DistanceToPlayer = 1,
        DistanceToAim = 2,
        DistanceToCamera = 3,
        DistanceToAimAndCamera = 4,
        ViewArea = 5,
        Height = 6,
        Allways = 7,
    }

    [RTTI.Serializable(0xB27CD4880D277733, GameType.DS)]
    public enum EDSSharedGimmickType : int8
    {
        Dummy = 0,
        Physics = 1,
        Animation = 2,
        BreakableWeak = 3,
        BreakableNormal = 4,
        BreakableStrong = 5,
        TriggerAnimation = 6,
        SimpleTrigger = 7,
        HeavyPhysics = 8,
        BreakableVeryWeak = 9,
        MuleSensor = 10,
        AutoDoor = 11,
        OneTimeTrigger = 12,
        RainyAnimation = 13,
        Pendulum = 14,
        PendulumHard = 15,
        PDBAnimation = 16,
        VehicleTriggerAnimation = 17,
        HeavyPhisicsWithSound = 18,
    }

    [RTTI.Serializable(0x59D655F320B6C7A7, GameType.DS)]
    public enum EDSShellDamageType : int32
    {
        MortarShell = 0,
        BomberBomb = 1,
    }

    [RTTI.Serializable(0xC1E913541F27C362, GameType.DS)]
    public enum EDSSignboardType : int8
    {
        Type1001 = 0,
        Type1002 = 1,
        Type1003 = 2,
        Type1004 = 3,
        Type1005 = 4,
        Type1006 = 5,
        Type1007 = 6,
        Type1008 = 7,
        Type1009 = 8,
        Type1010 = 9,
        Type1011 = 10,
        Type1012 = 11,
        Type1013 = 12,
        Type1014 = 13,
        Type1015 = 14,
        Type2001 = 15,
        Type2002 = 16,
        Type2003 = 17,
        Type2004 = 18,
        Type2005 = 19,
        Type2007 = 20,
        Type2008 = 21,
        Type2009 = 22,
        Type2010 = 23,
        Type2011 = 24,
        Type2013 = 25,
        Type2014 = 26,
        Type2015 = 27,
        Type2016 = 28,
        Type2017 = 29,
        Type2018 = 30,
        Type2019 = 31,
        Type2020 = 32,
        Type3001 = 33,
        Type3002 = 34,
        Type3003 = 35,
        Type3004 = 36,
        Type3004_l = 37,
        Type3005 = 38,
        Type3005_l = 39,
        Type3007 = 40,
        Type3007_l = 41,
        Type3010 = 42,
        Type3011 = 43,
        Type3012 = 44,
        Type3013 = 45,
        Type3014 = 46,
        Type3015 = 47,
        Type3016 = 48,
        Type3017 = 49,
        Type3018 = 50,
        Type3019 = 51,
        Type3020 = 52,
        Type3021 = 53,
        Type3022 = 54,
        Type4001 = 55,
        Type4002 = 56,
        Type4003 = 57,
        Type4004 = 58,
        Type4005 = 59,
        Type4010 = 60,
        Type4011 = 61,
        Type4012 = 62,
        Type4013 = 63,
        Type4014 = 64,
        Type4015 = 65,
        Type4016 = 66,
        Type4017 = 67,
        Type4018 = 68,
        Type5001 = 69,
        Type5002 = 70,
        Type6001 = 71,
        Type6002 = 72,
        Type6003 = 73,
        Type6004 = 74,
        Type6005 = 75,
        Type6007 = 76,
        Type6008 = 77,
        Type6010 = 78,
        Type6011 = 79,
        Type7003 = 80,
    }

    [RTTI.Serializable(0xD6DAE70C66EACA38, GameType.DS)]
    public enum EDSSmokeMissileMoverType : int8
    {
        HormingMissle = 0,
        TarBomb = 1,
        WaterGun = 2,
        PeelDebri = 3,
        TitanHomingMissile = 4,
    }

    [RTTI.Serializable(0xA43D1E1BAE8DFEAA, GameType.DS)]
    public enum EDSSpecialReportUIType : int32
    {
        Default = 0,
        Sub = 1,
        Main = 2,
        RouteExploration = 3,
        RouteNotExploration = 4,
        TotalDelivery = 5,
    }

    [RTTI.Serializable(0xDD92B8E7CEA2F065, GameType.DS)]
    public enum EDSStaffCreditType : int8
    {
        TrueED = 0,
        FakeED = 1,
    }

    [RTTI.Serializable(0x26559F8D45D5EBBE, GameType.DS)]
    public enum EDSStrandObjectSeType : int8
    {
        None = 0,
        Building = 1,
        SmallRock = 2,
        LargeRock = 3,
        SmallCar = 4,
        LargeCar = 5,
    }

    [RTTI.Serializable(0x9ACF8A4A17C17F71, GameType.DS)]
    public enum EDSStructureMarkerType : int8
    {
        None = 0,
        DeliveryPoint = 1,
        Preppers = 2,
        SafetyHouse = 3,
        Post = 4,
        __0 = 5,
        Charger = 6,
        WatchTower = 7,
        RainShelter = 8,
        MulePost = 9,
        RoadRebuilder = 10,
        Bridge = 11,
        Ladder = 12,
        Zipline = 13,
        Rope = 14,
        Camouflage = 15,
    }

    [RTTI.Serializable(0xFEDED351FFD6EAF0, GameType.DS)]
    public enum EDSSuitPartsCategory : int8
    {
        None = 0,
        ActiveSkeleton = 1,
        Mask = 2,
        Odradek = 3,
        Glove = 4,
        Harmonica = 5,
        Shield = 6,
        Hood = 7,
        Accessory = 8,
    }

    [RTTI.Serializable(0x98C8775C827F6DAE, GameType.DS)]
    public enum EDSSuitPartsId : int8
    {
        None = 0,
        ActiveSkeleton = 1,
        Mask = 2,
        Odradek = 3,
        BBCareTool = 4,
        BalanceSkeleton = 5,
        SpeedSkeleton = 6,
        PowerSkeleton = 7,
        PowerGlove = 8,
        Harmonica = 9,
        Shield = 10,
        ShieldLv2 = 11,
        OtterHood = 12,
        GlassesA = 13,
        GlassesB = 14,
        Cap = 15,
        FakeHolo = 16,
        HeatParts = 17,
        ShieldGold = 18,
        ShieldLv2Gold = 19,
        ShieldShilver = 20,
        ShieldLv2Shilver = 21,
        BalanceSkeletonShilver = 22,
        BalanceSkeletonGold = 23,
        BalanceSkeletonLv2 = 24,
        BalanceSkeletonLv3 = 25,
        SpeedSkeletonShilver = 26,
        SpeedSkeletonGold = 27,
        SpeedSkeletonLv2 = 28,
        SpeedSkeletonLv3 = 29,
        PowerSkeletonShilver = 30,
        PowerSkeletonGold = 31,
        PowerSkeletonLv2 = 32,
        PowerSkeletonLv3 = 33,
        ShieldLv3 = 34,
        ShieldLv4 = 35,
        SantaCap = 36,
        BalanceSkeletonLv2Shilver = 37,
        BalanceSkeletonLv2Gold = 38,
        BalanceSkeletonLv3Shilver = 39,
        BalanceSkeletonLv3Gold = 40,
        SpeedSkeletonLv2Shilver = 41,
        SpeedSkeletonLv2Gold = 42,
        SpeedSkeletonLv3Shilver = 43,
        SpeedSkeletonLv3Gold = 44,
        PowerSkeletonLv2Shilver = 45,
        PowerSkeletonLv2Gold = 46,
        PowerSkeletonLv3Shilver = 47,
        PowerSkeletonLv3Gold = 48,
        __0 = 49,
        __1 = 50,
        __2 = 51,
        __3 = 52,
        __4 = 53,
        ShieldLv3Gold = 54,
        ShieldLv4Gold = 55,
        ShieldLv3Shilver = 56,
        ShieldLv4Shilver = 57,
        __5 = 58,
    }

    [RTTI.Serializable(0x60ED870E3F0E4172, GameType.DS)]
    public enum EDSTakeLikeReason : int16
    {
        Unset = -1,
        Other = 17,
        FromBB = 6,
        FromPorter = 7,
        FromBridges = 9,
        FromPreppers = 10,
        FromBT = 11,
        ByDonation = 12,
        PickUpOfflineBaggages = 3,
        DeliverNpcBaggages = 0,
        PickUpOnlineBaggages = 4,
        DeliverOnlineUserBaggages = 1,
        DonationUsed = 13,
        BuildingUsed = 14,
        ContributeToConstruction = 15,
        FromOnlineUser = 16,
        DeliveryTogetherTotalWeight = 2,
        DiscardingChiralContaminants = 5,
    }

    [RTTI.Serializable(0xD34B42C3833E71F1, GameType.DS)]
    public enum EDSTempraryStorageLocation : int8
    {
        None = 0,
        BackpackHanger = 1,
        WaistHanger = 2,
        GrenadePouchCase = 3,
    }

    [RTTI.Serializable(0x644D744DF002800A, GameType.DS)]
    public enum EDSTerminalShelfType : int8
    {
        Shelf = 0,
        BeltConveyor = 1,
    }

    [RTTI.Serializable(0x9C254D05BABC0123, GameType.DS)]
    public enum EDSThrowableInspectorBehavior : int8
    {
        Default = 0,
        Suspend = 1,
        Remove = 2,
        DoNothing = 3,
    }

    [RTTI.Serializable(0xE49974EC2557DE9A, GameType.DS)]
    public enum EDSTimerControlMode : int8
    {
        CustomControl = 0,
        PlayTimeSyncControl = 1,
    }

    [RTTI.Serializable(0x108D0DFA8394FC2F, GameType.DS)]
    public enum EDSTipsGroup : int8
    {
        Unknown = 0,
        Menu = 1,
        Tips = 2,
        Controll = 3,
        Player_Controll = 4,
        Player_Move = 5,
        Player_Physical = 6,
        Player_Battery = 7,
        Player_Sensor = 8,
        Player_Search = 9,
        BB = 10,
        Knot_Space = 11,
        TimeFall = 12,
        Environment = 13,
        Onsen = 14,
        Enemy_Mule = 15,
        Enemy_Mule_Act = 16,
        Enemy_Terrorist_Act = 17,
        Enemy_BT = 18,
        NPC_Porter = 19,
        OrderType = 20,
        SimpleOrder = 21,
        SupplyRequest = 22,
        Baggage = 23,
        Baggage_Case = 24,
        Baggage_Type = 25,
        Baggage_PartialDelivery = 26,
        Baggage_Entrust = 27,
        Baggage_Lost = 28,
        SignBoard = 29,
        PCC = 30,
        Construction = 31,
        Construction_VersionUp = 32,
        Construction_StrengtheningCooperation = 33,
        Construction_Holo = 34,
        Construction_Repairing = 35,
        Construction_Bandwidth = 36,
        Facility_Cargo = 37,
        Material = 38,
        Friendship = 39,
        Like = 40,
        Highway = 41,
        Vehicle = 42,
        Garage = 43,
        FloatingCarts = 44,
        Tools = 45,
        Item = 46,
        UtilityPouch = 47,
        Equipment = 48,
        Backpack = 49,
        MemoryChip = 50,
        Waste = 51,
        Pass_Facility = 52,
        ShereBox = 53,
        PrivateBox = 54,
        Equipment_Create = 55,
        RestoreBaggagge = 63,
        FastTravel = 57,
        Cuff = 58,
        Cuff_MAP = 59,
        Cuff_WeatherForecast = 60,
        Mail = 61,
        BridgeLink = 62,
        PrivateRoom = 56,
        Rest = 64,
        BreakNG = 65,
        Save = 66,
        Deifficulty = 67,
        PhotoMode = 68,
        Other = 69,
        DebugOnly = 70,
    }

    [RTTI.Serializable(0xD1E4AFC500B890EA, GameType.DS)]
    public enum EDSTipsType : int8
    {
        TelopTips = 0,
        LoadingTips = 1,
        Interview = 2,
        Epigraph = 3,
    }

    [RTTI.Serializable(0xD69C63A1707FD938, GameType.DS)]
    public enum EDSTipsUnlockType : int8
    {
        None = 0,
        MissionClear = 1,
        ConnectQpid = 2,
        GameClear = 3,
        FriendshipLevel3 = 4,
        FriendshipLevel4 = 5,
        FriendshipLevel5 = 6,
        MemoryChip = 7,
    }

    [RTTI.Serializable(0xEFA08A1AD6DEE648, GameType.DS)]
    public enum EDSTodoNodeGroupRule : int32
    {
        Sequential = 0,
        And = 1,
        Or = 2,
    }

    [RTTI.Serializable(0xF954638D3C18C0D0, GameType.DS)]
    public enum EDSUIActionLocalizedId : int32
    {
        None = 0,
        PickUp = 1,
        LoadOn = 2,
        Build = 3,
        CancelBuild = 4,
        RideOn = 5,
        RideOff = 6,
        Kick = 7,
        InteractTerminal = 8,
        Transfer = 9,
        DeliveryMaterial = 10,
        GrabCart = 11,
        VehicleAttach = 12,
        MissionList = 13,
        Rumble = 14,
        PickUpR = 15,
        PickUpL = 16,
        GazerCut = 17,
        Carry = 18,
        CarryOff = 19,
        CartDetach = 21,
        CartAttach = 22,
        CartLink = 23,
        CartGetOff = 24,
        CartToBaggage = 25,
        BaggageToCart = 26,
        RopeFighting = 27,
        Climb = 28,
        ClimbToDown = 29,
        StepOn = 30,
        Rolling = 31,
        EludeToStepOn = 32,
        EludeToFall = 33,
        Dummy = 34,
        UsrLocation = 35,
        SearchBaggage = 36,
        SetMarker = 37,
        UnSetMarker = 38,
        UseZipline = 39,
        SendLike = 40,
        GetCollectible = 41,
        TakeABreak = 42,
        RepairShoes = 43,
        ChangeShoes = 44,
        TakeASleep = 45,
        TakeAMassageFoot = 46,
        TakeAMassageShoulder = 47,
        TakeASleepToWake = 48,
        Struggle = 49,
        Catch = 50,
        CatchRight = 51,
        CatchLeft = 52,
        PickUpToBackPack = 53,
        PickUpToBackPackFromHand = 54,
        ToStand = 55,
        LadderToBaggage = 56,
        LadderGrabIn = 57,
        LadderGrabOut = 58,
        CatchRope = 59,
        ThrowRope = 60,
        Detonate = 61,
        SprayFront = 62,
        SprayBack = 63,
        LockOnActivate = 64,
        LockOnDeactivate = 65,
        SubMagazineActivate = 66,
        SubMagazineDeactivate = 67,
        AppeaseBB = 68,
        StopAppeaseBB = 69,
        ShakeBBSilently = 70,
        ShakeBBSilently_R2 = 71,
        SubjectiveLookBBPod = 72,
        Return = 73,
        MarkerFocus = 74,
        ZiplineFocus = 75,
        ExitPrivateRoom = 76,
        DirectExitPrivateRoom = 77,
        UseShower = 78,
        ShowEquipmentShelf = 79,
        UseBBTool = 80,
        FastTravel = 81,
        UseWC = 82,
        ToUrinate = 83,
        ToFeces = 84,
        UseWashBasin = 85,
        ShowSuit = 86,
        CustomizeBackpack = 87,
        UseTerminal = 88,
        ShowTable = 89,
        WatchFigure = 90,
        ShowDreamCatcher = 91,
        UpDown = 92,
        Zoom = 93,
        CreateSignboard = 94,
        ReadSignboard = 95,
        LeftStick_Left = 97,
        LeftStick_Right = 98,
        LeftStick_Up = 99,
        LeftStick_Down = 100,
        RightStick_Left = 101,
        RightStick_Right = 102,
        RightStick_Up = 103,
        RightStick_Down = 104,
        Hacking = 105,
        UseWatchTower = 106,
        __0 = 107,
        MoveRight = 108,
        MoveLeft = 109,
        TerminalLocked = 110,
        SecondJump = 111,
        StopBreathHandL = 112,
        StopBreathHandR = 113,
        LevelUpDestroy = 114,
        BuildRoad = 115,
        WearCap = 116,
        WearGlasses = 117,
        DrinkBeer = 118,
        EatCryptobiosis = 119,
        RemoveCap = 121,
        RemoveGlasses = 122,
        Hug = 123,
        SwapBodyBag = 124,
        PutIntoBodyBag = 125,
        Jump = 126,
        JumpAttack = 127,
        UseCamouflageHolo = 128,
        DestroyConstruction = 129,
        UseRainShelterCoatingSpray = 130,
        GuardModeL = 131,
        GuardModeR = 132,
        GuardModeDouble = 133,
        LookBB = 134,
        StopLookingBB = 135,
        WeatheredConstruction = 136,
        TakeAOnsen = 137,
        PaddlingLeftSide = 138,
        PaddlingRightSide = 139,
        Swing = 140,
        RainShelter = 141,
        __1 = 142,
        __2 = 143,
        __3 = 144,
        MemoriesWithCliff = 145,
        VsWarriors = 146,
        VsWarriors1 = 147,
        VsWarriors2 = 148,
        VsWarriors3 = 149,
        RetrieveBaggages = 150,
        ZiplineSelect = 151,
        ZiplineCancel = 152,
        ZiplineGetOff = 153,
        __4 = 154,
        HiggsGuardMode = 155,
        HiggsSwing = 156,
        HiggsHeadbutt = 157,
        FakeHoloCancel = 158,
        OpenDoor = 159,
        ZoomIn = 160,
        ZoomOut = 161,
        GuardModeStopDouble = 162,
        VehicleMuleDrag = 163,
        InteractConstruction = 164,
        DestroyLadder = 165,
        DestroyRope = 166,
        DestroyVehicle = 167,
        EnterPrivateRoom = 168,
        Move = 169,
        Camera = 170,
        WatchTowerCancel = 171,
        ConstructionOpenMap = 172,
        LoadData = 173,
        ReturnFromKnotSpace = 174,
        BuildLadder = 175,
        BuildRope = 176,
        __5 = 177,
        TakeABreakCrouch = 178,
        WashBasinAction = 179,
        ZiplineRideOff = 180,
        __6 = 181,
        ChangeColor = 182,
        RopeParry = 183,
        PutCart = 184,
        PickupCart = 185,
        PutVehicle = 186,
        PickupVehicle = 187,
        BuildAim = 188,
        AppeaseLou = 189,
        StopAppeaseLou = 190,
        ShakeLouSilently = 191,
        ShakeLouSilently_R2 = 192,
        SubjectiveLookLouPod = 193,
        UseLouTool = 194,
        LookLou = 195,
        SwapBodybagAndBaggage = 196,
        PickMama = 197,
        PickArtist = 198,
        DropDeadbodyBag = 199,
        DropMama = 200,
        DropArtist = 201,
        BuildAimLadderRope = 202,
        ChangeBridgeLength = 203,
        WearGlassesGordon = 204,
        WearValve = 205,
        WearHeadCrab = 206,
        RemoveGlassesGordon = 207,
        RemoveValve = 208,
        RemoveHeadCrab = 209,
    }

    [RTTI.Serializable(0x47AE80FA56961618, GameType.DS)]
    public enum EDSUIAimHUDComponentAnimeType : int32
    {
        Unset = -1,
        Idle = 0,
        SwitchToEffective = 1,
        SwitchToDisabled = 2,
        BulletShot = 3,
        BulletReload = 4,
        BulletEmpty = 5,
        BulletEmpty_CanRefill = 6,
        BulletEmpty_NoCanRefill = 7,
        ChangeBulletType = 8,
        ShotFailure_EmptyBullet = 9,
        ShotFailure_EmptyBullet_CanRefill = 10,
        ShotFailure_EmptyBullet_NoCanRefill = 11,
        ShotFailure_EmptyBlood = 12,
        ShotFailure_EmptyChiralCrystal = 13,
        MaxCharge = 14,
        Charging = 15,
        EmptyCharge = 16,
        PlayerStateFireGun = 18,
        LifeProtection = 17,
        AdrenalineMode = 19,
        ConstructionPreparation = 20,
        LungWarningStateFirst = 21,
        LungWarningStateFinal = 22,
        ConsciousnessWarningStateFirst = 23,
        ConsciousnessWarningStateFinal = 24,
        NoMaskAtPoisonGasZone = 25,
        MaskAtPoisonGasZone = 26,
        StanDamage = 27,
        GivenLike = 28,
        DisLike = 29,
    }

    [RTTI.Serializable(0x168EA767F54E0AAF, GameType.DS)]
    public enum EDSUIAimHUDComponentSocketAutoAlignment : int32
    {
        DoNot = 0,
        TopUp = 1,
    }

    [RTTI.Serializable(0x9C2C2B816998157D, GameType.DS)]
    public enum EDSUIAimHUDSocketAnimeType : int32
    {
        Unset = 0,
    }

    [RTTI.Serializable(0x5A1B89B5964FD524, GameType.DS)]
    public enum EDSUIBulletType : int8
    {
        None = 8,
        StandardBullet = 0,
        BloodBullet = 1,
        ChiralBullet = 2,
        FragBullet = 3,
        SlipBullet = 4,
        SleepBullet = 5,
        StanBullet = 6,
        Urination = 7,
    }

    [RTTI.Serializable(0xCD630BF55C36CE7A, GameType.DS)]
    public enum EDSUIButtonIconStyleType : int32
    {
        NORMAL = 0,
        HOLD = 1,
        MASH = 2,
    }

    [RTTI.Serializable(0x6C82F0C43DB706C6, GameType.DS)]
    public enum EDSUICatalogueRadioType : int8
    {
        Invalid = -1,
        NewItem = 0,
        CatalogueUnlock = 1,
    }

    [RTTI.Serializable(0x1A2E7E813BFD8672, GameType.DS)]
    public enum EDSUICommonDialogueSetting : int8
    {
        OK = 0,
        OK_Cancel = 1,
        Yes = 2,
        Yes_No = 3,
        Yes_No_Cancel = 4,
        NonButton = 5,
        Timer = 6,
    }

    [RTTI.Serializable(0x1799654C04F4BD70, GameType.DS)]
    public enum EDSUICommonTelopHUDSound : int8
    {
        Landmark_Bridges = 0,
        Landmark_CheckPoint = 1,
        Landmark_Mission = 2,
        Landmark_Preppers = 3,
        Tutorial_Intro = 4,
        None = 5,
    }

    [RTTI.Serializable(0x1E0F8ED961EFC444, GameType.DS)]
    public enum EDSUICommonTelopType : int32
    {
        TELOP_COMMON = 0,
        TELOP_LANDMARK = 1,
        TELOP_TUTORIAL = 2,
        TELOP_MUSIC = 3,
        TELOP_STAFF = 4,
        TELOP_CHARACTER = 5,
    }

    [RTTI.Serializable(0x5D69C7169F7B35F2, GameType.DS)]
    public enum EDSUIConstructionOverrideMarkerType : int8
    {
        DontOverride = 0,
        Crematory = 1,
        RelayStation = 2,
        WindFarm = 3,
        WeatherStation = 4,
        MamaFacility = 5,
        HeartmanFacility = 6,
        CrossFacility = 7,
        RainFarm = 8,
        Area04Terminal = 9,
    }

    [RTTI.Serializable(0x877EA0DC713F88B2, GameType.DS)]
    public enum EDSUIDPadStateType : int32
    {
        SLEEP = 0,
        WAKE = 1,
        STANDBY = 2,
        FULL = 3,
        NONE = 4,
    }

    [RTTI.Serializable(0xCA52C7BAF0CFD505, GameType.DS)]
    public enum EDSUIDeviceMapDemoDeliveryPointEvent : int8
    {
        Invalid = 0,
        ShowQpidArea = 1,
        HideQpidArea = 2,
        ShowDeliveryPointIcon = 3,
        HideDeliveryPointIcon = 4,
        ShowDeliveryPointLine = 5,
        HideDeliveryPointLine = 6,
    }

    [RTTI.Serializable(0x3D808BCB66833FE9, GameType.DS)]
    public enum EDSUIDeviceMapDemoEvent : int8
    {
        Invalid = 0,
        FocusMap = 1,
        ShowTerminalIcon = 2,
        HideTerminalIcon = 3,
        ShowArea04 = 4,
        ShowArea04Terminal = 5,
        HideArea04 = 6,
        ShowCraterEffect = 7,
        ChangeZoomScale = 8,
        ShowLeftAndRightBlind = 9,
        HideLeftAndRightBlind = 10,
        ShowMiddleKnotImage = 11,
        HideMiddleKnotImage = 12,
        ShowSouthKnotImage = 13,
        HideSouthKnotImage = 14,
    }

    [RTTI.Serializable(0xD7A687AF68B186E4, GameType.DS)]
    public enum EDSUIDeviceMapMenuIconFilterItem : int8
    {
        DeliveryPoint = 0,
        MainMission = 1,
        RoadRebuilder = 2,
        Enemy = 3,
        BaggageBodyBag = 4,
        Online = 5,
        OnlineGoalDeliveryBaggageTag = 6,
        OnlineGoalDeliveryBaggageNoTag = 7,
        OnlineDeliveryBaggageTag = 8,
        OnlineDeliveryBaggageNoTag = 9,
        OnlineBaggageTag = 10,
        OnlineBaggageNoTag = 11,
        OnlineBaggageBrokenTag = 12,
        OnlineBaggageBrokenNoTag = 13,
        OnlineLadder = 14,
        OnlineFieldRope = 15,
        OnlinePost = 16,
        OnlineWatchTower = 17,
        OnlineBridge = 18,
        OnlineCharger = 19,
        OnlineRainShelter = 20,
        OnlineSafetyHouse = 21,
        OnlineZipline = 22,
        __0 = 23,
        OnlineSignboard = 24,
        OnlineBike = 25,
        OnlineTruck = 26,
        OnlineCart = 27,
        Mission = 28,
        MissionGoalDeliveryPoint = 29,
        MissionGoalDeliveryBaggageTag = 30,
        MissionGoalDeliveryBaggageNoTag = 31,
        Structure = 32,
        Ladder = 33,
        FieldRope = 34,
        Post = 35,
        WatchTower = 36,
        Bridge = 37,
        Charger = 38,
        RainShelter = 39,
        SafetyHouse = 40,
        Zipline = 41,
        __1 = 42,
        Signboard = 43,
        Vehicle = 44,
        Bike = 45,
        Truck = 46,
        Cart = 47,
        Collectible = 48,
        ChiralCrystal = 49,
        Cryptobiosis = 50,
        ShoeSoleGrass = 51,
        MemoryChip = 52,
        Baggage = 53,
        DeliveryBaggageTag = 54,
        DeliveryBaggageNoTag = 55,
        BaggageTag = 56,
        BaggageNoTag = 57,
        BaggageBrokenTag = 58,
        BaggageBrokenNoTag = 59,
    }

    [RTTI.Serializable(0x55D83468CE16D6D0, GameType.DS)]
    public enum EDSUIEquipFuncDamageIconType : int16
    {
        Invalid = 0,
        NonKill = 1,
        Kill = 2,
    }

    [RTTI.Serializable(0x9469A459FF8DBCC6, GameType.DS)]
    public enum EDSUIEquipFuncIcon : int8
    {
        None = 0,
        BT = 1,
        Kill = 2,
        NonKill = 3,
        Structure = 4,
        StaminaCare = 5,
        RepairTool = 6,
        BloodCare = 7,
        CommonTool = 8,
        Shoes = 9,
        Battery = 10,
        Odradek = 11,
        SuitParts = 12,
    }

    [RTTI.Serializable(0x1DC921366A9F3F6, GameType.DS)]
    public enum EDSUIEquipFuncIconSlotType : int32
    {
        Unknown = -1,
        AntiPersonnel = 0,
        AntiBt = 1,
    }

    [RTTI.Serializable(0x7ADD59C3F813CD1D, GameType.DS)]
    public enum EDSUIEquipFuncSpecialIconType : int16
    {
        None = 0,
        Reveal = 1,
        Blind = 2,
        Bind = 3,
        Mental = 4,
        Slip = 5,
    }

    [RTTI.Serializable(0x6B04F7E4FC53201A, GameType.DS)]
    public enum EDSUIFadeColorType : int8
    {
        Black = 0,
        White = 1,
        Auto = 2,
    }

    [RTTI.Serializable(0x2105567177B8560, GameType.DS)]
    public enum EDSUIFadeTimeType : int8
    {
        Short = 0,
        Middle = 1,
        Long = 2,
        SuperLong = 3,
        Immediately = 4,
        Custom = 5,
    }

    [RTTI.Serializable(0xE2153F6D5FA6DAB9, GameType.DS)]
    public enum EDSUIFadeType : int8
    {
        FadeIn = 0,
        FadeOut = 1,
    }

    [RTTI.Serializable(0x1CF20A3A2464850E, GameType.DS)]
    public enum EDSUIHUDElementExpanderPivot : int32
    {
        TopLeft = 0,
        TopRight = 1,
        BottomRight = 2,
        BottomLeft = 3,
        Center = 4,
    }

    [RTTI.Serializable(0xCB0ABA843D83C430, GameType.DS)]
    public enum EDSUIHUDImageTextureCoordsSelect : int8
    {
        TextrureCoords = 0,
        MaskTextrureCoords = 1,
    }

    [RTTI.Serializable(0x6B948103FDF9CA5B, GameType.DS)]
    public enum EDSUIHUDLineSizerPivot : int32
    {
        Start = 0,
        End = 1,
        Center = 2,
    }

    [RTTI.Serializable(0xC7FF54B7D97415B2, GameType.DS)]
    public enum EDSUIHUDLogicElementExpanderPivot : int32
    {
        TopLeft = 0,
        TopRight = 1,
        BottomRight = 2,
        BottomLeft = 3,
        Center = 4,
    }

    [RTTI.Serializable(0x875486A0DA75CAD1, GameType.DS)]
    public enum EDSUIHUDLogicElementLineSizerPivot : int32
    {
        Start = 0,
        End = 1,
        Center = 2,
    }

    [RTTI.Serializable(0x906D451EF43788DB, GameType.DS)]
    public enum EDSUIHudLineMode : int32
    {
        FitLineSize = 0,
        OriginalImageSize = 1,
    }

    [RTTI.Serializable(0x7A3703B595CF9255, GameType.DS)]
    public enum EDSUIImageDisplayHUDDescType : int8
    {
        Common = 0,
        Episode = 1,
        TitleLogo = 2,
    }

    [RTTI.Serializable(0xB62579F59F2D1D27, GameType.DS)]
    public enum EDSUIInfoLogHUDColorType : int8
    {
        Normal = 0,
        Caution = 1,
        Warning = 2,
        Like = 3,
        OnlineLike = 4,
    }

    [RTTI.Serializable(0x196A21B2026807B, GameType.DS)]
    public enum EDSUIInfoLogHUDDisplaySoundType : int8
    {
        Mail = 0,
        None = 1,
    }

    [RTTI.Serializable(0xDFF364F3BD0DBC69, GameType.DS)]
    public enum EDSUIInfoLogHUDIconType : int8
    {
        Default = 0,
        Mail = 1,
        None = 2,
    }

    [RTTI.Serializable(0xD5D8D093C0F7019F, GameType.DS)]
    public enum EDSUIMapInternalDrawerType : int8
    {
        DrawDefaultOffscreen = 0,
        DrawHeightOffscreen = 1,
        DrawScreen = 2,
        DrawHighPriorityIcons = 3,
    }

    [RTTI.Serializable(0x4198396465706492, GameType.DS)]
    public enum EDSUIMenuCountNumberStartType : int8
    {
        OnShow = 0,
        OnFocused = 1,
        Scripted = 2,
        UniqueIdChanged = 3,
    }

    [RTTI.Serializable(0xB1E40919636E1060, GameType.DS)]
    public enum EDSUIMenuSoundCategory : int8
    {
        None = 0,
        Device = 1,
        DeviceTop = 2,
        DeviceDelivery = 3,
        DeviceMission = 4,
        DeviceWish = 5,
        DeviceSystem = 6,
        DeviceBaggage = 7,
        DeviceRadio = 8,
        DevicePreparation = 9,
        DeviceHandOver = 10,
        DeviceConstruction = 11,
        DeviceBridgesID = 12,
        DeviceFastTravel = 13,
        DeviceMail = 14,
        DeviceBackPack = 15,
        DeviceVehicle = 16,
        DeviceMapDemo = 17,
        DeviceQpidResult = 18,
        DeviceMusic = 19,
        DevicePrivateRoomColor = 20,
        DeviceFinalResult = 21,
        Database = 22,
        DatabaseTips = 23,
        DatabaseArchive = 24,
        DatabaseInfoLog = 25,
        DatabaseMemoryChip = 26,
        CommonDialogue = 27,
        InGamePause = 28,
        DemoPause = 29,
        Controller = 30,
        Config = 31,
        PhotoMode = 32,
        MissionResult = 33,
        Signboard = 34,
        Uniform = 35,
        CliffMemories = 36,
        PrivateRoomPhoto = 37,
        Title = 38,
        GameOver = 39,
        Birthday = 40,
        Install = 41,
        LoadingScreen = 42,
        SplashScreen = 43,
    }

    [RTTI.Serializable(0x6EAB5DD57CD1A362, GameType.DS)]
    public enum EDSUIMessageEventType : int32
    {
        Unknown = 0,
        OnOpen = 1,
        OnClose = 2,
        OnLifeView = 3,
        OnStaminaView = 4,
        OnMissionListDialogOpen = 5,
        OnMissionListDialogAccept = 6,
        OnMissionListDialogCancel = 7,
        OnReceiveMail = 8,
        OnOpenedMail = 9,
        OnPageChange = 10,
        OnConstructionSignboard = 11,
    }

    [RTTI.Serializable(0x1F04E0CB99AC6E66, GameType.DS)]
    public enum EDSUIMessageSenderType : int32
    {
        Unknown = 0,
        DSUIAimHUD = 1,
        DSUITutorialTelopHUD = 2,
        DSUILandmarkTelopHUD = 3,
        DSUIOperationGuideTelopHUD = 4,
        DSUIMissionTelopHUD = 5,
        DSUIWeaponSelectorHUD = 6,
        DSUIDeviceMissionMenu = 7,
        DSUIMissionResultMenu = 8,
        DSUIDeviceMailMenu = 9,
        DSUISignboardMenu = 10,
        DSUIInfoLogHUD = 11,
    }

    [RTTI.Serializable(0x8E5D396787E84C2E, GameType.DS)]
    public enum EDSUIMissionRadioType : int8
    {
        Invalid = -1,
        MissionAccept = 0,
        MissionUnlock = 1,
        PreparationRadio = 2,
        PreparationCatalogue = 3,
        MainMissionList = 4,
        MissionTypeSelect = 5,
        DeliveryMenuCanHandOver = 6,
        OpenMissionDetail = 7,
        OpenMissionDetailTab = 8,
    }

    [RTTI.Serializable(0x38DCFCE9A5D4518C, GameType.DS)]
    public enum EDSUIMusicTelopDisplayMode : int32
    {
        DEFAULT = 0,
        MUSIC_PLAYER = 1,
    }

    [RTTI.Serializable(0xC9163BB502070D12, GameType.DS)]
    public enum EDSUIOperationGuideStyleType : int32
    {
        STRONG = 0,
        WEAK = 1,
    }

    [RTTI.Serializable(0xE582092382B07B27, GameType.DS)]
    public enum EDSUIProgressDirectionType : int32
    {
        UNSET = 0,
        HORIZONTAL = 1,
        VERTICAL = 2,
    }

    [RTTI.Serializable(0x359C25FC0E58203D, GameType.DS)]
    public enum EDSUIRadioCategory : int8
    {
        Mission = 0,
        MissionBaggage = 1,
        Catalogue = 2,
        Resident = 3,
        Common = 4,
    }

    [RTTI.Serializable(0x9FF78CAB8070BAC6, GameType.DS)]
    public enum EDSUIStaffCreditHUDRowType : int8
    {
        _0 = 0,
        _1C = 1,
        _1C_s = 2,
        _1G = 3,
        _2CC = 4,
        _2CC_s = 5,
        _2CC_sm = 6,
        _2CC_w = 7,
        _2CC_sw = 8,
        _2GG = 9,
        _2RL = 10,
        _3CCC = 11,
        _3CCC_s = 12,
        _3GGG = 13,
        _4CCCC = 14,
        _4RLLL = 15,
        _4RLRL = 16,
        _6RLLRLL = 17,
        _STRAND = 18,
        _LL_SIEA = 20,
        _LL_SIEE1 = 21,
        _LL_SIEE2 = 22,
        _LL_SIEE3 = 23,
        _LL_SIEJ = 24,
        _LL_SIE_Asia = 25,
    }

    [RTTI.Serializable(0xB48110CB668EF416, GameType.DS)]
    public enum EDSUIStaffTelopPositionType : int32
    {
        CUSTOM = 0,
        TOP_LEFT = 1,
        TOP_RIGHT = 2,
        BOTTOM_LEFT = 3,
        BOTTOM_RIGHT = 4,
        CENTER = 5,
    }

    [RTTI.Serializable(0xC9DAC47CA1065AA3, GameType.DS)]
    public enum EDSUIStarGraphCategory : int8
    {
        BridgeLink = 0,
        Safety = 1,
        Service = 2,
        Delivery = 3,
        Speed = 4,
    }

    [RTTI.Serializable(0x84FEF787991E5D2B, GameType.DS)]
    public enum EDSUITelopDisplayPriorityType : int32
    {
        Priority_1st_MISSION = 0,
        Priority_2nd_COMMON_PRIORITY = 1,
        Priority_3rd_COMMON = 2,
    }

    [RTTI.Serializable(0x6E7BFBE498498E45, GameType.DS)]
    public enum EDSUITimerHUDAnimeType : int32
    {
        Unset = -1,
        Idle = 0,
        WarningFirstEffect = 11,
        WarningSecondEffect = 12,
        RichWarningFirstEffect = 13,
        RichWarningSecondEffect = 14,
        TimeoutEffect = 15,
        TimerWipesOut = 1,
        Mute = 2,
        ModeExchange = 3,
        StandardDisplayEffect = 9,
        WarningDisplayEffect = 10,
        FourSecondsBeforeFlash = 4,
        ThreeSecondsBeforeFlash = 5,
        TwoSecondsBeforeFlash = 6,
        OneSecondsBeforeFlash = 7,
        ZeroSecondsBeforeFlash = 8,
    }

    [RTTI.Serializable(0x40AC61FFCBD1C932, GameType.DS)]
    public enum EDSUIUnlockDialogTiming : int8
    {
        None = 0,
        Unique = 1,
        FriendlyLvUp1 = 2,
        FriendlyLvUp2 = 3,
        FriendlyLvUp3 = 4,
        FriendlyLvUp4 = 5,
        FriendlyLvUp5 = 6,
        AfterMissionResult = 7,
        AfterQpidConnect = 8,
        AfterQpidConnect2 = 9,
        AfterQpidConnect_NewCommer = 10,
        AfterSimpleMissionResult = 11,
        AfterDonationResult = 12,
        AfterDonationMemorychip = 13,
        StarGraphComplete = 14,
        GameClear = 15,
        DeliveryOfLostBaggage = 16,
    }

    [RTTI.Serializable(0xF294C0FE6AD83D78, GameType.DS)]
    public enum EDSUIUnlockUIFlagType : int32
    {
        None = 0,
        UnlockHandCuffsDeviceMenu = 1,
        UnlockWeatherNews = 2,
        UnlockWeaponSelectorHUD = 3,
        UnlockChiralNetworkUI = 4,
        UnlockRadio = 5,
        UnlockPremiumDelivery = 6,
        UnlockStructureLevelUp = 7,
        UnlockStolenCataloguePrinter = 8,
        UnlockAutobotMissionMenu = 9,
        UnlockDeliveryLostBaggageMenu = 10,
        UnlockGiftBaggageMenu = 11,
        UnlockGiftCrystalMenu = 12,
        UnlockGiftMemoryChipMenu = 13,
        UnlockToPublicBoxMenu = 14,
        UnlockFromPublicBoxMenu = 15,
        UnlockPrivateRoom = 16,
        UnlockCatalogueMenu = 17,
        UnlockVehicleMenu = 18,
        UnlockStructureCustom = 19,
        UnlockStructureRecover = 20,
        UnlockBBStressGauge = 21,
        UnlockBBStressGaugeDeactive = 22,
        UnlockShoesLifeGauge = 23,
        UnlockChiralNumDisplay = 24,
        UnlockMailMenu = 25,
        UnlockSupplyRequestMenu = 26,
        UnlockBridgeLinkMenu = 27,
        UnlockPauseControllerMenuHandCuffs = 28,
        UnlockPauseControllerMenuTool = 29,
        UnlockPauseControllerMenuStructureDevice = 30,
        UnlockPauseControllerMenuCombat = 31,
        UnlcokPauseControllerMenuVehicle = 32,
        UnlcokPauseControllerMenuBullet = 33,
        UnlockQpidUI = 34,
        UnlockSimpleMission = 35,
        UnlockAimHUD = 36,
        UnlockUpWeaponSelectorHUD = 37,
        UnlockRightWeaponSelectorHUD = 38,
        UnlockDownWeaponSelectorHUD = 39,
        UnlockLeftWeaponSelectorHUD = 40,
        UnlockPauseMenuMissionTodo = 41,
        UnlockResultOrderPersonDisplay = 42,
        UnlockTakeOutMaterialMenu = 43,
        UnlockGiftTrashMenu = 44,
        UnlockEntrustMenu = 45,
        UnlockDPadHUDSunglassesIcon = 46,
        UnlockDPadHUDSunglassesAndShoesIcon = 47,
        UnlockDPadHUDSunglassesAndShoesAndSuitPartsIcon = 48,
        UnlockDeliverySupplyMenu = 49,
        UnlockCliffMemoriesWorldWarII = 50,
        UnlockCliffMemoriesVietnamWar = 51,
        UnlockCliffMemoriesFlashbackMenu = 52,
        UnlockUniformMenuCapColorChange = 53,
        UnlockUniformMenuGlassesAColorChange = 54,
        UnlockUniformMenuGlassesBColorChange = 55,
        UnlockUniformMenuSuitsColorChange = 56,
        UnlockTimerHUD = 57,
        UnlockTarBeltCraterInMap = 58,
        UnlockMountainKnotPondInMap = 59,
        UnlockShoesChange = 60,
        UnlockSimpleMissionForM560 = 61,
    }

    [RTTI.Serializable(0xEF7A8C9294B40B7C, GameType.DS)]
    public enum EDSUIWeaponSelectorCategoryType : int32
    {
        INVALID = 0,
        MAINWEAPON = 1,
        ITEM = 2,
        EQUIPMENT = 3,
        MAGAZINE = 4,
    }

    [RTTI.Serializable(0x43388D2CCAF632C3, GameType.DS)]
    public enum EDSUIWeaponSelectorTutorialEventType : int32
    {
        OPEN__EQUIPMENT = 0,
        CLOSE = 1,
        SELECT__SUNGLASSES = 2,
        COMMAND__UNSET_SUNGLASSES = 3,
    }

    [RTTI.Serializable(0xC9D708F49BB099DB, GameType.DS)]
    public enum EDSUnsaveReason : int32
    {
        None = 0,
        BTEncounter = 1,
        MuleArea = 2,
        DontCarryNuclearWeapon = 4,
        BeginCountDown = 8,
        PoisonArea = 16,
        ForbidSaveAndBreakByFact = 32,
        Warrior = 64,
        BossBattle = 128,
        EventsRestricted = 256,
        RideVehicle = 512,
        FallingSam = 1024,
        MovingSamBaggages = 2048,
        Adrenaline = 4096,
        InWater = 8192,
        CancelSavingByGame = 16384,
        CancelSavingBySystem = 32768,
        IsLoading = 65536,
        DisableSave = 131072,
        AstralMode = 262144,
        BaggageInTarSwamp = 524288,
        InDanger = 1048576,
        BusySaveSystem = 2097152,
        BusyBaggageManager = 4194304,
        BusyLostBaggageSystem = 8388608,
        ZipLine = 16777216,
    }

    [RTTI.Serializable(0x2734CE315E6654E5, GameType.DS)]
    public enum EDSVirtualButton : int32
    {
        kDSVB_None = 0,
        kDSVB_Decide = 1,
        kDSVB_Cancel = 2,
        kDSVB_COMPASS_FOCUS_LR = 3,
        kDSVB_L_STICK = 4,
        kDSVB_L_STICK_LEFT = 5,
        kDSVB_L_STICK_RIGHT = 6,
        kDSVB_L_STICK_UP = 7,
        kDSVB_L_STICK_DOWN = 8,
        kDSVB_L_STICK_ROTATE = 9,
        kDSVB_R_STICK = 10,
        kDSVB_R_STICK_LEFT = 11,
        kDSVB_R_STICK_RIGHT = 12,
        kDSVB_R_STICK_UP = 13,
        kDSVB_R_STICK_DOWN = 14,
        kDSVB_R_STICK_ROTATE = 15,
        kDSVB_UP_DOWN = 16,
        kDSVB_RUMBLE = 17,
        kDSVB_CAMERA_ACTION = 18,
        kDSVB_SelectorItemSelect = 19,
        kDSVB_SelectorPageChange = 20,
        kDSVB_MoveToX = 21,
        kDSVB_LookToX = 22,
        kDSVB_MotionSensor_Shake = 23,
    }

    [RTTI.Serializable(0x9461498917B8447, GameType.DS)]
    public enum EDSWDBakeBlendMode : int32
    {
        AlphaBlend = 0,
        Add = 1,
        Multiply = 3,
        Substruct = 2,
    }

    [RTTI.Serializable(0xCC82F7E7AB1CB10F, GameType.DS)]
    public enum EDSWDBakeType : int32
    {
        Color = 0,
        Height = 1,
        Normal = 2,
    }

    [RTTI.Serializable(0xE54A8DD502CDE41D, GameType.DS)]
    public enum EDSWDMPrimitiveBakeDataMode : int32
    {
        None = 0,
        Height = 1,
        Displacement = 2,
        Height_And_Displacement = 3,
    }

    [RTTI.Serializable(0x1952747BD337711E, GameType.DS)]
    public enum EDSWDMPrimitiveType : int32
    {
        Hemisphere = 0,
        Parabola = 1,
        Flat = 2,
    }

    [RTTI.Serializable(0x8CB3869A86D5A221, GameType.DS)]
    public enum EDSWarriorMechParticleEventType : int8
    {
        SpawnToPatritle = 0,
        SpawnToSubstance = 1,
        SubstanceToParticle = 2,
        Vanish = 3,
    }

    [RTTI.Serializable(0x40EC26B256A206D6, GameType.DS)]
    public enum EDSWarriorType : int32
    {
        WARRIOR_WW1 = 0,
        WARRIOR_WW2 = 1,
        WARRIOR_VW = 2,
        WARRIOR_AFG = 3,
    }

    [RTTI.Serializable(0x733B217207B378C5, GameType.DS)]
    public enum EDSWeaponCategory : int8
    {
        None = 0,
        Gun = 1,
        Throw = 2,
        Spread = 3,
        Place = 4,
        Physical = 5,
        Builder = 6,
        Strand = 7,
        Urination = 8,
    }

    [RTTI.Serializable(0xA03C40F2BDF27E20, GameType.DS)]
    public enum EDSWeaponConnectPoint : int32
    {
        Invalid = -1,
        StandardMuzzle = 0,
        SuppressorMuzzle = 1,
        Ejection = 2,
        AttachmentGrenaderMuzzle = 3,
        AttachmentFlashLight = 4,
        AttachmentEjection = 5,
        Muzzle1Of4 = 6,
        Muzzle2Of4 = 7,
        Muzzle3Of4 = 8,
        Muzzle4Of4 = 9,
        BackpackHanger = 10,
        Pod = 11,
        General0 = 12,
        General1 = 13,
    }

    [RTTI.Serializable(0x28C5EB822E6F18BF, GameType.DS)]
    public enum EDSWeaponId : int8
    {
        None = 0,
        AssaultRifle = 1,
        AssaultRifleLv2 = 2,
        AssaultRifleLv3 = 3,
        AssaultRifleLv4 = 4,
        Grenade = 5,
        BloodGrenade = 6,
        BloodGrenadeLv1Extend = 7,
        BloodGrenadeLv2 = 8,
        ElectricalGrenadeLv1 = 9,
        ElectricalGrenadeLv2 = 10,
        ElectricalGrenadePlace = 11,
        CoatingSpray = 12,
        SmokeGrenade = 13,
        SmokeGrenadeLv2 = 14,
        FreezeGrenade = 15,
        TranquilizerGun = 16,
        AmnioticFluidGrenade = 17,
        ExGrenade0 = 18,
        ExGrenade1 = 19,
        ExGrenade1Plus = 20,
        ExGrenade2 = 21,
        BolaGun = 22,
        BolaGunLv2 = 23,
        ShotGun = 24,
        ShotGunLv2 = 25,
        ShotGunLv3 = 26,
        HandGun = 27,
        HandGunLv2 = 28,
        HandGunLv3 = 29,
        BloodHandGun = 30,
        BloodHandGunLv2 = 31,
        AmelieHandGun = 32,
        C4 = 33,
        GazerBalloon = 34,
        SamBall = 35,
        SamBallLv2 = 36,
        Builder = 37,
        BuilderLv2 = 38,
        Grenader = 39,
        AirBurstGrenader = 40,
        SlipGrenader = 62,
        Strand = 41,
        RubberAssaultRifle = 42,
        RubberAssaultRifleLv2 = 43,
        RubberAssaultRifleLv3 = 44,
        RubberAssaultRifleLv4 = 45,
        RubberShotGun = 46,
        RubberShotGunLv2 = 47,
        RubberShotGunLv3 = 48,
        Ladder = 49,
        LadderLv2 = 50,
        Rope = 51,
        RopeLv2 = 52,
        RopeLv3 = 53,
        StickyGun = 54,
        FourConsecutiveMissile = 55,
        SpreadMissile = 56,
        HologramDevice = 57,
        Urination = 58,
        EnemyAssaultRifle = 59,
        HiggsAssaultRifle = 60,
        MultiRod = 61,
        EnemyRubberAssaultRifle = 63,
        Ww1Rifle = 64,
        Ww1ShotGun = 65,
        Ww1Grenade = 66,
        Ww1MachineGun = 67,
        Ww2SubmachineGun = 68,
        Ww2Rifle = 69,
        Ww2Missile = 70,
        Ww2SmokeGrenade = 71,
        VietnamAssault = 72,
        VietnamAssaultWithGrenader = 73,
        VietnamMachinegun = 74,
        VietnamGrenade = 75,
        CliffRifle = 76,
        AfghanRifle = 77,
        HiggsKnife = 78,
        DemensAssaultRifle = 79,
        DemensShotGun = 80,
        TrekkingPole = 81,
        Ww2MissileType2 = 82,
        EnemyGrenade = 83,
        Ww2Grenade = 84,
        AfghanGrenade = 85,
        Ww2AirPlaneMachinegun = 86,
        Ww2HeavyMachinegun = 87,
        DemensElectricalGrenade = 88,
        __0 = 89,
    }

    [RTTI.Serializable(0x232E38F4D1813EF3, GameType.DS)]
    public enum EDSWeaponMotionType : int32
    {
        Invalid = -1,
        Neutral = 0,
        Ready = 1,
        EquipStart = 2,
        EquipEnd = 3,
        EquipPose = 4,
        Fire = 5,
        Reload = 6,
        CoverReloadLeft = 7,
        CoverReloadRight = 8,
        AimStart = 9,
        AimEnd = 10,
        FastReload = 11,
        PrivateRoomDisplay = 12,
        SquatReload = 13,
        SquatCoverReloadLeft = 14,
        SquatCoverReloadRight = 15,
        AttachmentReload = 16,
        AmmoTypeSwitching = 17,
    }

    [RTTI.Serializable(0xD68BCB741DCDAFAE, GameType.DS)]
    public enum EDSWeaponPartsId : int8
    {
        None = 0,
        FlashLight = 1,
    }

    [RTTI.Serializable(0xC3FEED36CFBE4BD6, GameType.DS)]
    public enum EDSWeaponType : int8
    {
        None = 0,
        MainWeapon = 1,
        SubWeapon = 2,
    }

    [RTTI.Serializable(0x2DC053182DD925D6, GameType.DS)]
    public enum EDSWeatherForecastType : int8
    {
        Rainy = 0,
        Cloudy = 1,
        Average = 2,
        RainyOnly = 3,
        CloudyOnly = 4,
        HeavyRainOnly = 5,
        SunnyOnly = 6,
        RainyNotHeavyRain = 7,
        AverageNotHeavyRain = 8,
        RainyForMule = 9,
        CloudyForMule = 11,
        AverageForMule = 10,
        RainyNotHeavyRainForMule = 12,
        AverageNotHeavyRainForMule = 13,
        Invalid_NotChange_ = 14,
    }

    [RTTI.Serializable(0xAD75D077520C8A36, GameType.DS)]
    public enum EDSWeatherRegionIndex : int8
    {
        OutOfRegion = 0,
        Region01 = 1,
        Region02 = 2,
        Region03 = 3,
        Region04 = 4,
        Region05 = 5,
        Region06 = 6,
        Region07 = 7,
        Region08 = 8,
        Region09 = 9,
        Region10 = 10,
        Region11 = 11,
        Region12 = 12,
        Region13 = 13,
        Region14 = 14,
        Region15 = 15,
        Region16 = 16,
        Region17 = 17,
        Region18 = 18,
        Region19 = 19,
        Region20 = 20,
        Region21 = 21,
        Region22 = 22,
        Region23 = 23,
        Region24 = 24,
        Region25 = 25,
        Region26 = 26,
        Region27 = 27,
        Region28 = 28,
        Region29 = 29,
        Region30 = 30,
        Region31 = 31,
        Region32 = 32,
        Region33 = 33,
        Region34 = 34,
        Region35 = 35,
        Region36 = 36,
        Region37 = 37,
        Region38 = 38,
        Region39 = 39,
        Region40 = 40,
        Region41 = 41,
        Region42 = 42,
        Region43 = 43,
        Region44 = 44,
        Region45 = 45,
        Region46 = 46,
        Region47 = 47,
        Region48 = 48,
        Region49 = 49,
        Region50 = 50,
        Region51 = 51,
        Region52 = 52,
        Region53 = 53,
        Region54 = 54,
        Region55 = 55,
        Region56 = 56,
        Region57 = 57,
        Region58 = 58,
        Region59 = 59,
        Region60 = 60,
        Region61 = 61,
        Region62 = 62,
        Region63 = 63,
    }

    [RTTI.Serializable(0xA9A3794BBB050833, GameType.DS)]
    public enum EDSWeatherStateType : int8
    {
        None = 0,
        Sunny = 1,
        Cloudy = 2,
        Rainny = 3,
        RainnyBt = 4,
        Knot = 5,
        Subspace = 6,
        HeavyRain = 7,
        DarkCloudy = 8,
        RainyBtTar = 9,
    }

    [RTTI.Serializable(0xD5DC7D87B133CD2F, GameType.DS)]
    public enum EDSWhaleAttackPathType : int8
    {
        Jump = 0,
        TailBlow = 1,
        BodyBlow = 2,
        TidalWave = 3,
        Capture = 4,
        Cutter = 5,
        Missle = 6,
        Jump_Boss_ = 7,
        TailBlow_Boss_ = 8,
        BodyBlow_Boss_ = 9,
        TidalWave_Boss_ = 10,
        Capture_Boss_ = 11,
        Cutter_Boss_ = 12,
        Missle_Boss_ = 13,
    }

    [RTTI.Serializable(0x5A994A0A09A3659A, GameType.DS)]
    public enum EDSWhaleMovePathActionType : int8
    {
        None = 0,
        SmokeMissile = 1,
        TarBomb = 2,
        TarCutter = 3,
        GoldenHunter = 4,
        SmallJumpA = 5,
        SmallJumpB = 6,
        FlyRoar = 7,
        TarBeam2 = 8,
        TarBeamEx = 9,
    }

    [RTTI.Serializable(0x183BBDFC0BA70D67, GameType.DS)]
    public enum EDSWhaleMovePathType : int8
    {
        NormalMove = 0,
        NormalFly = 1,
        SmokeMissileFly = 2,
        TarBombFly = 3,
        GoldenHunterFly = 4,
        TarBeam2Fly = 5,
        TarBeamExFly = 6,
        MultiAttackFly = 7,
    }

    [RTTI.Serializable(0x5693448D6C3F4DBF, GameType.DS)]
    public enum EDSZiplineType : int8
    {
        Normal = 0,
        Large = 1,
        NetNormal = 2,
        NetLarge = 3,
        StageNormal = 4,
        StageLarge = 5,
    }

    [RTTI.Serializable(0x451A5B8C8AAFF841, GameType.DS)]
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

    [RTTI.Serializable(0x1635C5268F821B8, GameType.DS)]
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

    [RTTI.Serializable(0x6261180EFFAE400F, GameType.DS)]
    public enum EDamageModifierTypeFilter : int32
    {
        Any = 0,
        Equals = 1,
        Not_Equals = 2,
    }

    [RTTI.Serializable(0xF4473264D4C3369, GameType.DS)]
    public enum EDebrisEntityLifetime : int8
    {
        Dispensable = 0,
        OtherEntity = 1,
    }

    [RTTI.Serializable(0xA13DC9447CD9C177, GameType.DS)]
    public enum EDecalAlignment : int32
    {
        AlignToImpactDirection = 0,
        AlignToWorldUpAxis = 1,
    }

    [RTTI.Serializable(0x2B4FCB5E28663DF9, GameType.DS)]
    public enum EDecalManagerEventType : int8
    {
        ChangeAlpha = 1,
        Delete = 0,
    }

    [RTTI.Serializable(0x6C0354EF63B06714, GameType.DS)]
    public enum EDecalProjectionMode : int32
    {
        ProjectImpactNormal = 0,
        ProjectImpactDir = 1,
        ProjectSurfaceHeuristic = 2,
    }

    [RTTI.Serializable(0x18C5E60448AD55A8, GameType.DS)]
    public enum EDecalVariableSource : int32
    {
        None = 0,
        Fade = 1,
        Emissive = 2,
        UVFrameIndex = 3,
        ColorType = 4,
    }

    [RTTI.Serializable(0x4836DAA96F71C95D, GameType.DS)]
    public enum EDefaultDataStorageType : int8
    {
        None = 0,
        Binary = 1,
        ObjectRef = 2,
        UUIDRef = 3,
    }

    [RTTI.Serializable(0x6FD217A2B7F522E9, GameType.DS)]
    public enum EDelayLineTapIndex : int32
    {
        Tap_0 = 0,
        Tap_1 = 1,
        Tap_2 = 2,
        Tap_3 = 3,
    }

    [RTTI.Serializable(0xF05C6CF2742249E, GameType.DS)]
    public enum EDensityJobBakeType : int32
    {
        NoShaderCompilation = 0,
        FullConversion = 1,
    }

    [RTTI.Serializable(0x7718FC8906307384, GameType.DS)]
    public enum EDensityJobType : int32
    {
        SingleMap = 0,
        Full = 1,
    }

    [RTTI.Serializable(0xF44FD1B7C0613505, GameType.DS)]
    public enum EDepthOfFieldQuality : int8
    {
        Gameplay = 0,
        Cinematic = 1,
        Default = 0,
    }

    [RTTI.Serializable(0x34D2026DE219C10, GameType.DS)]
    public enum EDeviceFunction : int32
    {
        Invalid = -1,
        PrimaryFire = 4,
        SwitchFire = 5,
        NextAmmo = 6,
        PreviousAmmo = 7,
        MeleeWeaponPrimaryAttack = 8,
        MeleeWeaponSecondaryAttack = 9,
        MeleeWeaponDashAttack = 10,
        ZoomSwitch = 11,
        ZoomModeSwitch = 12,
        Aim = 13,
        SprintToggle = 14,
        Jump = 15,
        Use = 16,
        Relocate = 17,
        Loot = 18,
        PickupWeapon = 19,
        Reload = 20,
        Crouch = 21,
        Slide = 22,
        Cover = 23,
        Vault = 24,
        Dodge = 25,
        DropWeapon = 26,
        NextWeapon = 27,
        SwimDescend = 28,
        SwimAscend = 29,
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
        CounterBucking1 = 40,
        CounterBucking2 = 41,
        PrimaryContextualAction = 42,
        SecondaryContextualAction = 43,
        TertiaryContextualAction = 44,
        RequestVoiceComm = 45,
        AAGunFirePrimary = 46,
        AAGunFireSecondary = 47,
        AAGunZoomSwitch = 48,
        SelectUp = 49,
        SelectDown = 50,
        SelectLeft = 51,
        SelectRight = 52,
        CharacterScreenCampaign = 53,
        CharacaterScreenOnline = 54,
        IngameMainMenu = 57,
        OptionScreenCampaign = 55,
        OptionScreenOnline = 56,
        MountHorse = 58,
        DismountHorse = 59,
        DismountHorseSpecial = 60,
        MountSpeedUp = 61,
        MountSpeedDown = 62,
        MountEmergencyStop = 63,
        MountRangedAttack = 64,
        MountMeleeAttack = 65,
        MountSecondaryMeleeAttack = 66,
        CallHorse = 67,
        MountDuckRider = 68,
        InventoryNextWeapon = 69,
        InventorySelection = 70,
        InventoryUseTool = 72,
        InventoryToggleToolWheel = 73,
        InventoryAmmoCraft = 71,
        InventoryQuickUseToolLeft = 74,
        InventoryQuickUseToolRight = 75,
        InventoryQuickUseToolUp = 76,
        InventoryQuickUseToolDown = 77,
        ProtoRight = 78,
        ProtoLeft = 79,
        ProtoUp = 80,
        ProtoDown = 81,
        ProtoCross = 82,
        ProtoSquare = 83,
        ProtoTriangle = 84,
        ProtoCircle = 85,
        ProtoShoulderLeft1 = 86,
        ProtoShoulderLeft2 = 87,
        ProtoShoulderRight1 = 88,
        ProtoShoulderRight2 = 89,
        ProtoLeftAnalog = 90,
        ProtoRightAnalog = 91,
        ProtoSelect = 92,
        ProtoStart = 93,
        Tag = 94,
        Untag = 95,
        UntagAll = 96,
        Focus = 97,
        FocusTagStatusInfo = 98,
        FocusWeaponSelect = 99,
        FocusUp = 100,
        FocusDown = 101,
        FocusLeft = 102,
        FocusRight = 103,
        BulletTime = 104,
        PlaceClimbGrip = 105,
        LureEnemy = 107,
        AudiologToggle = 108,
        DsPickup = 109,
        DsSubject = 110,
        DsHold = 111,
        DsShoot = 112,
        DsAction = 113,
        DsReload = 114,
        DsStance = 115,
        DsDodge = 116,
        DsDash = 117,
        DsStockChange = 118,
        DsDemoPause = 119,
        DsPhotoMode = 120,
        DsEcho = 121,
        DsDeviceButtonL1 = 122,
        DsDeviceButtonL2 = 123,
        DsDeviceButtonL3 = 124,
        DsDeviceButtonR1 = 125,
        DsDeviceButtonR2 = 126,
        DsDeviceButtonR3 = 127,
        DsDeviceButtonTriangle = 128,
        DsDeviceButtonSquare = 129,
        DsDeviceButtonCross = 130,
        DsDeviceButtonCircle = 131,
        DsDeviceButtonDPadLeft = 132,
        DsDeviceButtonDPadRight = 133,
        DsDeviceButtonDPadUp = 134,
        DsDeviceButtonDPadDown = 135,
        DsDeviceButtonOption = 136,
        DsDeviceButtonTouchPad = 137,
        DsDeviceButtonTouchPadLeft = 138,
        DsDeviceButtonTouchPadRight = 139,
        Confirm = 140,
        Cancel = 141,
        CampfireUserSave = 142,
    }

    [RTTI.Serializable(0xF5B64B51B14862C9, GameType.DS)]
    public enum EDischargeMethod : int32
    {
        Timed_discharge = 0,
        Dissipate_charge__interruptible_ = 1,
        Force_dissipate__non_interruptible_ = 2,
        Instant_reset = 3,
    }

    [RTTI.Serializable(0xCF86FAD530E2F299, GameType.DS)]
    public enum EDiscoveryState : int32
    {
        Completed = 3,
        Discovered = 2,
        Indicated = 1,
        Undiscovered = 0,
    }

    [RTTI.Serializable(0x397BF1974C04B6C2, GameType.DS)]
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

    [RTTI.Serializable(0xF7D667BF20F7F0B6, GameType.DS)]
    public enum EDrawHUDMode : int32
    {
        On = 0,
        Partially = 1,
        Off = 2,
    }

    [RTTI.Serializable(0xE12E702D19B35E80, GameType.DS)]
    public enum EDrawPartType : int32
    {
        Normal = 0,
        ShadowCasterOnly = 1,
    }

    [RTTI.Serializable(0x1A29227A3858E682, GameType.DS)]
    public enum EEcotopeSamplingMode : int8
    {
        EcotopeMapping = 0,
        EcotopeIndex = 1,
    }

    [RTTI.Serializable(0x257405E9739AD01B, GameType.DS)]
    public enum EElementAutoRotate : int32
    {
        None = 0,
        RotateToLight = 1,
        RotateToCentre = 2,
    }

    [RTTI.Serializable(0x594D45A8955FBC3D, GameType.DS)]
    public enum EElementColor : int32
    {
        GlobalColor = 0,
        CustomColor = 1,
        Spectrum = 2,
        Gradient = 3,
    }

    [RTTI.Serializable(0xC6E5AFDFA3BBC1BE, GameType.DS)]
    public enum EElementTranslation : int32
    {
        None = 0,
        Free = 1,
        HorizontalOnly = 2,
        VerticalOnly = 3,
        Custom = 4,
    }

    [RTTI.Serializable(0xDCB947D4613A5490, GameType.DS)]
    public enum EEmitAxis : int32
    {
        x = 0,
        y = 1,
        z = 2,
    }

    [RTTI.Serializable(0xAB6D365543595250, GameType.DS)]
    public enum EEmitterShape : int32
    {
        Sphere = 0,
        Box = 1,
        Ellipsoid = 2,
        Torus = 3,
        Mesh = 4,
    }

    [RTTI.Serializable(0x9D15D46468DDF141, GameType.DS)]
    public enum EEntityComponentSetMode : int8
    {
        Add_All_Components = 0,
        Add_Single_Component__Random_ = 1,
    }

    [RTTI.Serializable(0xD2A0C3CE20B25D2F, GameType.DS)]
    public enum EEntityImpostorDirection : int8
    {
        Forward = 0,
        Backward = 1,
    }

    [RTTI.Serializable(0x175D4144AEAFBADB, GameType.DS)]
    public enum EEntityImpostorType : int8
    {
        None = 0,
        Normal = 1,
    }

    [RTTI.Serializable(0x40E3BA087260C24, GameType.DS)]
    public enum EEntityLifetimeType : int8
    {
        Manual = 0,
        OtherEntity = 1,
        Scene = 2,
        Dispensable = 3,
    }

    [RTTI.Serializable(0x1028BD96E548306A, GameType.DS)]
    public enum EEnvelopeMode : int8
    {
        ASR = 0,
        ADSR = 1,
    }

    [RTTI.Serializable(0x712F1536332B28C, GameType.DS)]
    public enum EEnvironmentInteractionTexRes : int16
    {
        _32_x_32 = 32,
        _64_x_64 = 64,
        _128_x_128 = 128,
        _256_x_256 = 256,
        _512_x_512 = 512,
        _1024_x_1024 = 1024,
        _2048_x_2048 = 2048,
    }

    [RTTI.Serializable(0x7D4E64FB4CA842DA, GameType.DS)]
    public enum EEnvironmentInteractionWorldSize : int16
    {
        _4_x_4 = 4,
        _8_x_8 = 8,
        _16_x_16 = 16,
        _32_x_32 = 32,
        _64_x_64 = 64,
        _128_x_128 = 128,
        _256_x_256 = 256,
    }

    [RTTI.Serializable(0xC2E750CAB7C318FB, GameType.DS)]
    public enum EEquipSlotType : int8
    {
        Invalid = 0,
        None = 1,
        RangedWeapon = 2,
        MeleeWeapon = 3,
        HeavyWeapon = 4,
        Outfit = 5,
    }

    [RTTI.Serializable(0x196A3F158648C28, GameType.DS)]
    public enum EExertionAnimationEventTriggerType : int8
    {
        Trigger_at_start = 0,
        Trigger_continuous = 1,
        Trigger_on_stop = 2,
    }

    [RTTI.Serializable(0x19A33500F3CF36C, GameType.DS)]
    public enum EExposedCombatSituationSummary : int32
    {
        invalid = 0,
        relaxed = 1,
        suspicious = 2,
        identified_unknown = 3,
        identified_observed = 4,
    }

    [RTTI.Serializable(0x3FF7445E3541E348, GameType.DS)]
    public enum EFactConditionCompareOperator : int8
    {
        Equal = 0,
        NotEqual = 1,
        Greater = 2,
        GreaterOrEqual = 3,
        Lesser = 4,
        LesserOrEqual = 5,
    }

    [RTTI.Serializable(0x7C24C76627F85312, GameType.DS)]
    public enum EFactConditionContextMode : int8
    {
        Global = 0,
        Player = 1,
        Context = 2,
    }

    [RTTI.Serializable(0xEBC0B502B0A56159, GameType.DS)]
    public enum EFactionSetMode : int32
    {
        Name = 0,
        DefaultFaction = 1,
        NeutralFaction = 2,
    }

    [RTTI.Serializable(0x9348F9FA0FAF3B84, GameType.DS)]
    public enum EFalloffType : int32
    {
        No = 0,
        Linear = 1,
        Square = 2,
    }

    [RTTI.Serializable(0x59A2BD2E0762DAC0, GameType.DS)]
    public enum EFloating : int32
    {
        _0 = 0,
        left = 1,
        right = 2,
        center = 3,
    }

    [RTTI.Serializable(0x57FA319DB51894FD, GameType.DS)]
    public enum EFloorNrDirection : int8
    {
        Upwards = 0,
        Downwards = 1,
    }

    [RTTI.Serializable(0xAADA18796DC46CC7, GameType.DS)]
    public enum EFloorSlopeDetectionMethod : int32
    {
        InaccurateNormalBased = 0,
        Probes = 1,
    }

    [RTTI.Serializable(0x2DC9BF8B2774D465, GameType.DS)]
    public enum EFocusState : int8
    {
        PendingActivation = 2,
        Activating = 3,
        Activated = 4,
        Deactivating = 1,
        Deactivated = 0,
    }

    [RTTI.Serializable(0x39D6BFE6CAE6FBCB, GameType.DS)]
    public enum EForceBehaviour : int32
    {
        Vortex = 0,
        Attract = 1,
        Repel = 2,
        Push_Through = 3,
        Turbulence = 4,
        Push_Attract = 5,
    }

    [RTTI.Serializable(0x9EC7B2384B3972C5, GameType.DS)]
    public enum EForceFieldCategoryMask : int32
    {
        None = 0,
        Wind = 1,
        Particle = 2,
        Vegetation = 4,
        PBD = 8,
        Physics = 16,
        PlantInteraction = 32,
        PresetLocal = 30,
        PresetAll = 63,
    }

    [RTTI.Serializable(0xC98AC989525B4D75, GameType.DS)]
    public enum EForceFieldFilter : int32
    {
        All = 0,
        ForceFieldSamplerOnly = 1,
    }

    [RTTI.Serializable(0x7D6DDD43E884288D, GameType.DS)]
    public enum EForceFieldFlowDriver : int32
    {
        None = 0,
        Wind_speed = 1,
        Wind_speed_and_direction = 2,
        Directional_wind_speed = 3,
        Bidirectional_wind_speed = 4,
    }

    [RTTI.Serializable(0x20D55B62489AE253, GameType.DS)]
    public enum EForceFieldShape : int32
    {
        Sphere = 0,
        Box = 1,
    }

    [RTTI.Serializable(0xC1382540BDE1B305, GameType.DS)]
    public enum EForceType : int32
    {
        Flow = 0,
        Force = 1,
    }

    [RTTI.Serializable(0x52E00DD0CFE0AF92, GameType.DS)]
    public enum EForwardShadowCastMode : int32
    {
        Auto = 0,
        Enable = 1,
        Disable = 2,
    }

    [RTTI.Serializable(0x4CEB81B40FC52974, GameType.DS)]
    public enum EGameMode : int32
    {
        _0 = -1,
        __gm0 = 0,
        __gm1 = 1,
        __gm2 = 2,
        __gm3 = 3,
    }

    [RTTI.Serializable(0x99206C72F3FA5B75, GameType.DS)]
    public enum EGamepadDevice : int32
    {
        NoDevice = 0,
        DualShock4Device = 1,
        GamingInputDevice = 2,
        XInputDevice = 3,
        SteamInputDevice = 4,
        FakeInputDevice = 5,
    }

    [RTTI.Serializable(0x34C6343CAAA82D65, GameType.DS)]
    public enum EGamepadHardware : int32
    {
        NoHardware = 0,
        UnknownHardware = 1,
        DualShock4Hardware = 2,
        DualShock3Hardware = 3,
        XBoxOneHardware = 4,
        XBox360Hardware = 5,
        GenericXinputHardware = 6,
        SteamControllerHardware = 7,
        SwitchProHardware = 8,
    }

    [RTTI.Serializable(0xFD9374588C6E65F1, GameType.DS)]
    public enum EGender : int8
    {
        Male = 1,
        Female = 2,
    }

    [RTTI.Serializable(0xBAB609E6B6EE7B38, GameType.DS)]
    public enum EGestureBodyParts : int32
    {
        HEAD_AND_LEFT_HAND = 0,
        HEAD_AND_RIGHT_HAND = 1,
        FULLBODY_LEFT = 2,
        FULLBODY_RIGHT = 3,
    }

    [RTTI.Serializable(0x56EFC004F8653677, GameType.DS)]
    public enum EGestureDirection : int32
    {
        None = 0,
        Subject = 2,
        Target = 1,
    }

    [RTTI.Serializable(0xAE73405AB17746A6, GameType.DS)]
    public enum EGraphSoundUpdateRate : int32
    {
        Every_Synth_Frame = 1,
        Every_2nd_Synth_Frame = 2,
        Every_3rd_Synth_Frame = 3,
        Every_4th_Synth_Frame = 4,
        Every_8th_Synth_Frame = 8,
        Every_16th_Synth_Frame = 16,
        Every_32th_Synth_Frame = 32,
        Every_64th_Synth_Frame = 64,
    }

    [RTTI.Serializable(0x5175D0F0C0784E56, GameType.DS)]
    public enum EHAlign : int8
    {
        Default = 0,
        Left = 1,
        Center = 2,
        Right = 3,
    }

    [RTTI.Serializable(0x7BBFECEEA6FF83F9, GameType.DS)]
    public enum EHTTPRequestMethod : int32
    {
        GET = 1,
        POST = 2,
        PUT = 3,
    }

    [RTTI.Serializable(0x6595D3D3F92367A9, GameType.DS)]
    public enum EHUDBlendMode : int8
    {
        AlphaBlend = 0,
        AlphaAdd = 1,
    }

    [RTTI.Serializable(0x7232ECA4F60CEDA0, GameType.DS)]
    public enum EHUDImageMode : int32
    {
        Stretch = 0,
        Tile = 1,
        AutoSize = 2,
        AspectRatioPreserved = 3,
    }

    [RTTI.Serializable(0x4F409F19512A2756, GameType.DS)]
    public enum EHUDLayer : int32
    {
        PostMenu = 1,
    }

    [RTTI.Serializable(0x501D02F2AA56E0A2, GameType.DS)]
    public enum EHUDLogicElementExpanderAxes : int32
    {
        HorizontalOnly = 0,
        VerticalOnly = 1,
        BothSimultaneously = 2,
    }

    [RTTI.Serializable(0x6BA66C6848E372F, GameType.DS)]
    public enum EHUDLogicElementExpanderPivot : int32
    {
        TopLeft = 0,
        TopRight = 1,
        BottomRight = 2,
        BottomLeft = 3,
        Center = 4,
    }

    [RTTI.Serializable(0xD48291BB9393F8E3, GameType.DS)]
    public enum EHUDLogicElementFaderMode : int32
    {
        FadeIn = 0,
        FadeOut = 1,
    }

    [RTTI.Serializable(0xE8DF7F13B281EAD0, GameType.DS)]
    public enum EHUDShowOption : int8
    {
        Dynamic = 0,
        AlwaysOn = 1,
        AlwaysOff = 2,
        FocusOnly = 4,
    }

    [RTTI.Serializable(0x5448AC7401A2C2D0, GameType.DS)]
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

    [RTTI.Serializable(0xAA021392CE43302B, GameType.DS)]
    public enum EHUDTextAlign : int8
    {
        None = 0,
        Left = 1,
        Center = 2,
        Right = 3,
    }

    [RTTI.Serializable(0x5FDAD4EE39321DD8, GameType.DS)]
    public enum EHUDTextMode : int32
    {
        Unclipped = 0,
        AutoSize = 1,
        WordWrap = 2,
        WordWrapAutoSize = 3,
        AutoFitTextSize = 4,
    }

    [RTTI.Serializable(0xB2D622196081F405, GameType.DS)]
    public enum EHUDUnits : int32
    {
        Pixels = 0,
        Percentage = 1,
    }

    [RTTI.Serializable(0xECC88F51D21C6554, GameType.DS)]
    public enum EHealthRegenerationSettings : int32
    {
        Slow = 0,
        Normal = 1,
        Fast = 2,
    }

    [RTTI.Serializable(0x6668FD5E3BB5D9D9, GameType.DS)]
    public enum EHighLevelBehaviorDescription : int8
    {
        idling = 0,
        patrolling = 1,
        defending = 2,
        searching = 3,
        fleeing = 4,
        transporting = 5,
        combat = 6,
        scavenging = 7,
        unknown = 8,
    }

    [RTTI.Serializable(0x4151F1C68847603B, GameType.DS)]
    public enum EHitDirection : int32
    {
        Any = 0,
        Back = 1,
        Left = 2,
        Front = 3,
        Right = 4,
    }

    [RTTI.Serializable(0x441E5FF654CAFE3F, GameType.DS)]
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

    [RTTI.Serializable(0x9DEFFE596FE7BD79, GameType.DS)]
    public enum EHitReactionAccumulationType : int32
    {
        Impact_Severity = 0,
        Damage = 1,
    }

    [RTTI.Serializable(0x8983DEBA88F73304, GameType.DS)]
    public enum EHitReactionCycleMode : int32
    {
        Cycle = 0,
        Reset_Last = 1,
        Disable = 2,
    }

    [RTTI.Serializable(0x77BD6E1FF16105D4, GameType.DS)]
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
    }

    [RTTI.Serializable(0xE333D1A88C944F9, GameType.DS)]
    public enum EHumanoidDismountMovement : int32
    {
        Idle = 0,
        Moving = 1,
        Falling = 2,
        Dead = 3,
        Jumping = 4,
    }

    [RTTI.Serializable(0x5DCAA8454EF6A0ED, GameType.DS)]
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
        Left_Analog = 28,
        Right_Analog = 29,
    }

    [RTTI.Serializable(0xF2A372102AA8AE6A, GameType.DS)]
    public enum EImageCompressionMethod : int32
    {
        PerceptualData = 0,
        NormalData = 1,
        VariableData = 2,
        DefaultData = 3,
    }

    [RTTI.Serializable(0xCA564630080B732D, GameType.DS)]
    public enum EImpactEffectOverrideMode : int32
    {
        Add = 0,
        Replace = 1,
    }

    [RTTI.Serializable(0x3841266011B29EF1, GameType.DS)]
    public enum EIndirectLightingProbeHint : int32
    {
        Exclude = 0,
        UseVisualGeo = 1,
        UsePhysicsGeo = 2,
    }

    [RTTI.Serializable(0x1A5A412170059483, GameType.DS)]
    public enum EInfinityMode : int8
    {
        Constant = 0,
        Extrapolate = 1,
        Cycle = 2,
        CycleRelative = 3,
        Oscillate = 4,
    }

    [RTTI.Serializable(0x448E0AF2453E7D09, GameType.DS)]
    public enum EInventoryCategory : int8
    {
        Unspecified = -1,
        Weapons = 0,
        Tools = 1,
        Ammo = 2,
        Modifications = 3,
        Outfits = 4,
        Resources = 5,
        Special = 6,
        LootBoxes = 7,
        None = 9,
    }

    [RTTI.Serializable(0x7BEA514EEF6FD1E0, GameType.DS)]
    public enum EInventoryItemAddType : int8
    {
        Regular = 0,
        Transfer = 1,
        LoadSave = 2,
    }

    [RTTI.Serializable(0x797336779EBBC8C8, GameType.DS)]
    public enum EInventoryItemRarity : int8
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        VeryRare = 3,
    }

    [RTTI.Serializable(0xE6745797FF528D6F, GameType.DS)]
    public enum EInventoryItemRemoveType : int8
    {
        Destroy = 0,
        Transfer = 1,
        Drop = 2,
        Keep = 3,
    }

    [RTTI.Serializable(0xE867827034C97A90, GameType.DS)]
    public enum EJointOperation : int8
    {
        Add = 1,
        Set = 0,
        Ignore = 2,
    }

    [RTTI.Serializable(0xA77D2CCBBCA1E98E, GameType.DS)]
    public enum EJointSpace : int8
    {
        LocalSpace = 0,
        ModelSpace = 1,
    }

    [RTTI.Serializable(0x338A6BB8E194DAFF, GameType.DS)]
    public enum EKJPChromaticAberrationSampleCount : int32
    {
        _3_Samples = 3,
        _5_Samples = 5,
    }

    [RTTI.Serializable(0x63D24EF63257B4AE, GameType.DS)]
    public enum EKJPDistortionFalloffType : int32
    {
        Longer_Direction = 0,
        Shorter_Direction = 1,
        Horizontal_Direction = 2,
        Vertical_Direction = 3,
    }

    [RTTI.Serializable(0x14C0769F57771D72, GameType.DS)]
    public enum EKJPLightLocatorType : int8
    {
        Position = 0,
        RelativePosition = 1,
        Animated = 2,
    }

    [RTTI.Serializable(0xAD1F9FF7583E034, GameType.DS)]
    public enum EKJPLightningEventMode : int8
    {
        Game = 0,
        Manual = 1,
    }

    [RTTI.Serializable(0x9411C6B331BEA970, GameType.DS)]
    public enum EKeyCode : int32
    {
        _0 = 0,
        Esc = 1,
        _2 = 3,
        _3 = 4,
        Space = 5,
        _5 = 6,
        _6 = 7,
        _7 = 8,
        _8 = 9,
        _9 = 10,
        _10 = 11,
        _11 = 12,
        _12 = 13,
        _13 = 14,
        _14 = 15,
        _15 = 16,
        Enter = 17,
        Backspace = 18,
        Tab = 19,
        Left = 20,
        Right = 21,
        Up = 22,
        Down = 23,
        Home = 24,
        End = 25,
        PageUp = 26,
        PageDown = 27,
        Ins = 28,
        Del = 29,
        Pad_ = 30,
        Pad_0 = 31,
        Pad_1 = 32,
        Pad_2 = 33,
        PadEnter = 34,
        Pad0 = 35,
        Pad1 = 36,
        Pad2 = 37,
        Pad3 = 38,
        Pad4 = 39,
        Pad5 = 40,
        Pad6 = 41,
        Pad7 = 42,
        Pad8 = 43,
        Pad9 = 44,
        PadDel = 45,
        CapsLock = 46,
        PrintScreen = 47,
        ScrollLock = 48,
        NumLock = 49,
        Pause = 50,
        LeftAlt = 51,
        RightAlt = 52,
        LeftCtrl = 53,
        RightCtrl = 54,
        LeftShift = 55,
        RightShift = 56,
        LeftWinLogo = 57,
        RightWinLogo = 58,
        ContextMenu = 59,
        _59 = 60,
        _60 = 61,
        _61 = 62,
        _62 = 63,
        _63 = 64,
        _64 = 65,
        _65 = 66,
        _66 = 67,
        _67 = 68,
        _68 = 69,
        _69 = 70,
        _70 = 71,
        _71 = 72,
        _72 = 73,
        _73 = 74,
        _74 = 75,
        _75 = 76,
        _76 = 77,
        F1 = 78,
        F2 = 79,
        F3 = 80,
        F4 = 81,
        F5 = 82,
        F6 = 83,
        F7 = 84,
        F8 = 85,
        F9 = 86,
        F10 = 87,
        F11 = 88,
        F12 = 89,
        _0_0 = 90,
        _1_0 = 91,
        _2_0 = 92,
        _3_0 = 93,
        _4_0 = 94,
        _5_0 = 95,
        _6_0 = 96,
        _7_0 = 97,
        _8_0 = 98,
        _9_0 = 99,
        A = 100,
        B = 101,
        C = 102,
        D = 103,
        E = 104,
        F = 105,
        G = 106,
        H = 107,
        I = 108,
        J = 109,
        K = 110,
        L = 111,
        M = 112,
        N = 113,
        O = 114,
        P = 115,
        Q = 116,
        R = 117,
        S = 118,
        T = 119,
        U = 120,
        V = 121,
        W = 122,
        X = 123,
        Y = 124,
        Z = 125,
        CapsToggle = 126,
        NumToggle = 127,
        ScrollToggle = 128,
    }

    [RTTI.Serializable(0x30CEE692AAF77CAC, GameType.DS)]
    public enum ELanguage : int32
    {
        English = 1,
        Unknown = 0,
        Dutch = 6,
        German = 4,
        French = 2,
        Spanish = 3,
        Italian = 5,
        Portuguese = 7,
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
        English_UK = 22,
        Greek = 23,
        Czech = 24,
        Hungarian = 25,
    }

    [RTTI.Serializable(0xD1DEA84A604CCF6E, GameType.DS)]
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

    [RTTI.Serializable(0x3125CC21B0B1AE07, GameType.DS)]
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
        MulInvSrcColor = 21,
    }

    [RTTI.Serializable(0x92453CDD8FB8502A, GameType.DS)]
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
        leftstick_updown = 18,
        leftstick_leftright = 19,
        rightstick_updown = 20,
        rightstick_leftright = 21,
        cycleprevnext = 22,
        tableftright = 23,
        updown = 24,
        leftright = 25,
        up_down_left_right = 26,
        ds_squarehold_and_leftstick = 27,
        ds_squarehold_for_marker_remove = 28,
        ds_trianglehold_for_structure_destroy = 29,
        ds_touchpad_and_dualshock_gyro = 30,
        intel_left = 31,
        map_zoom = 32,
        switch_photoMode_operation = 33,
        photomode_lctrl = 34,
        map_shift_scroll = 35,
    }

    [RTTI.Serializable(0x58A67CBC959845BA, GameType.DS)]
    public enum ELensFlareTriggerFalloff : int32
    {
        Linear = 0,
        Smooth = 1,
        Exponential = 2,
    }

    [RTTI.Serializable(0xDDA988904A7C57F3, GameType.DS)]
    public enum ELensFlareTriggerMode : int32
    {
        ObjectPosition = 0,
        LightPosition = 1,
    }

    [RTTI.Serializable(0xDF69D6395DB05C73, GameType.DS)]
    public enum ELensFlareTriggerType : int32
    {
        FromBorder = 0,
        FromCentre = 1,
        FromLight = 2,
    }

    [RTTI.Serializable(0x7D4B500EDF014423, GameType.DS)]
    public enum ELightAreaType : int32
    {
        Point = 0,
        Disk = 1,
        Rect = 2,
    }

    [RTTI.Serializable(0xDF730EA39C15E444, GameType.DS)]
    public enum ELightCollectionIdentifierMode : int32
    {
        TimeOfDay = 0,
        NamedLightCollection = 1,
    }

    [RTTI.Serializable(0xC6A69AFA7AD2A016, GameType.DS)]
    public enum ELightSamplingResolution : int32
    {
        LightSamplingRes8x8 = 3,
        LightSamplingRes4x4 = 2,
        LightSamplingRes2x2 = 1,
        LightSamplingRes1x1 = 0,
    }

    [RTTI.Serializable(0xEFB35758C93C176F, GameType.DS)]
    public enum ELightUnitType : int32
    {
        Lumen = 0,
        Lux = 1,
    }

    [RTTI.Serializable(0x6B1CC97EFF088A9, GameType.DS)]
    public enum ELightbakeZoneOrientation : int32
    {
        WorldSpace = 0,
        BakeZoneSpace = 1,
    }

    [RTTI.Serializable(0xEED1740D649A6608, GameType.DS)]
    public enum ELightbakeZoneQuality : int32
    {
        Default = 0,
        High = 1,
    }

    [RTTI.Serializable(0xC45E86399F103111, GameType.DS)]
    public enum ELightbakeZoneRestriction : int32
    {
        AboveGround = 1,
        BelowGround = 0,
        Universal = 2,
        GroundLevel = 3,
    }

    [RTTI.Serializable(0x6659F739028E3EB3, GameType.DS)]
    public enum ELightmapEncodeColorScale : int32
    {
        Do_not_scale = 0,
        Scale_by_brightest_color = 1,
        Scale_so_one_pixel_in_100_is_clamped__10x10_ = 2,
        Scale_so_one_pixel_in_300_is_clamped__15x15_ = 3,
        Scale_so_one_pixel_in_1000_is_clamped__30x30_ = 4,
        Scale_so_one_pixel_in_3000_is_clamped__50x50_ = 5,
        Scale_so_one_pixel_in_10000_is_clamped__100x100_ = 6,
    }

    [RTTI.Serializable(0x5B8BDA3CA6E0D9B0, GameType.DS)]
    public enum ELocationType : int32
    {
        Global = 0,
        Local = 1,
    }

    [RTTI.Serializable(0xEE0F852A08DC1682, GameType.DS)]
    public enum ELookDirection : int32
    {
        None = 0,
        Subject = 2,
        Target = 1,
    }

    [RTTI.Serializable(0xE7762EF7D7C7B274, GameType.DS)]
    public enum ELoopMode : int32
    {
        Off = 0,
        On = 1,
        Hold = 2,
        PingPong = 3,
    }

    [RTTI.Serializable(0x7D601D08618768E6, GameType.DS)]
    public enum ELootDataIncrementType : int8
    {
        LootSlotLevel = 0,
        LootDataLevel = 1,
    }

    [RTTI.Serializable(0x67B73BEC3200B74C, GameType.DS)]
    public enum EMapZoneRevealAreaMode : int8
    {
        PlayerOnly = 0,
        MapOnly = 1,
        PlayerAndMap = 2,
    }

    [RTTI.Serializable(0x68B1263E2BE0C061, GameType.DS)]
    public enum EMapZoomLevel : int8
    {
        LowZoom = 0,
        MediumZoom = 1,
        HighZoom = 2,
    }

    [RTTI.Serializable(0xCE56B509004136E5, GameType.DS)]
    public enum EMarkerObjectiveType : int16
    {
        SimpleDestination = 0,
        Salvage = 1,
        DeliveryDestination = 2,
    }

    [RTTI.Serializable(0x2A745752926128EF, GameType.DS)]
    public enum EMaterialDebugType : int32
    {
        Static = 0,
        Dynamic = 1,
    }

    [RTTI.Serializable(0x9B2B179EA7DCC852, GameType.DS)]
    public enum EMeleeAttackRotationRestriction : int8
    {
        Allow = 0,
        ResetCombo = 1,
        Deny = 2,
    }

    [RTTI.Serializable(0x21A790626493E43, GameType.DS)]
    public enum EMeleeDamageImpulseDirectionType : int32
    {
        BoxMovementDirection = 0,
        Radial = 1,
        FixedToEntity = 2,
        FixedToParentEntity = 3,
    }

    [RTTI.Serializable(0xC94D8172DFB0EF76, GameType.DS)]
    public enum EMenuActionFocusType : int8
    {
        Target = 0,
        FirstChild = 1,
        LastChild = 2,
    }

    [RTTI.Serializable(0xF48EB2365CEB50D4, GameType.DS)]
    public enum EMenuAnimatableProperty : int8
    {
        OffsetX = 0,
        OffsetY = 1,
        OffsetZ = 2,
        Opacity = 3,
        FontScale = 4,
        TextureScale = 5,
        RotationX = 6,
        RotationY = 7,
        RotationZ = 8,
        ScaleX = 9,
        ScaleY = 10,
        Width = 11,
        Height = 12,
        ColorR = 13,
        ColorG = 14,
        ColorB = 15,
    }

    [RTTI.Serializable(0xE6CB94D11415F4B, GameType.DS)]
    public enum EMenuAnimationAction : int8
    {
        Start = 0,
        Pause = 1,
        Stop = 2,
        Skip = 3,
    }

    [RTTI.Serializable(0x956521AF39AE546C, GameType.DS)]
    public enum EMenuAnimationTrigger : int8
    {
        Idle = 0,
        FocusReceived = 3,
        FocusLost = 4,
        PageOpen = 5,
        PageLeave = 6,
        OnShow = 7,
        OnHide = 8,
        Scripted = 1,
        ScriptedInverted = 2,
    }

    [RTTI.Serializable(0xAF88AE8BC116226D, GameType.DS)]
    public enum EMenuBadgeCategory : int8
    {
        Collectables = 21,
        CatalogueRobots = 22,
        CatalogueDataCubes = 23,
    }

    [RTTI.Serializable(0x275F5C3391C9BAF0, GameType.DS)]
    public enum EMenuBlendMode : int8
    {
        Invalid = 0,
        Write = 1,
        Add = 2,
        AlphaBlend = 3,
        AlphaAdd = 4,
        PreMulAlphaBlend = 5,
    }

    [RTTI.Serializable(0x729A100D3B05E1C9, GameType.DS)]
    public enum EMenuCameraProperty : int8
    {
        PosX = 0,
        PosY = 1,
        PosZ = 2,
        LookAtX = 3,
        LookAtY = 4,
        LookAtZ = 5,
        HorizontalFOV = 6,
    }

    [RTTI.Serializable(0x8B0F22B584218F57, GameType.DS)]
    public enum EMenuEvent : int32
    {
        Unset = -1,
        OnPressAccept = 8,
        OnPressCancel = 9,
        OnPressStart = 10,
        OnPressDpadUp = 11,
        OnPressDpadDown = 12,
        OnPressDpadLeft = 13,
        OnPressDpadRight = 14,
        OnPressUp = 15,
        OnPressDown = 16,
        OnPressLeft = 17,
        OnPressRight = 18,
        OnPressLeftAnalog = 19,
        OnPressRightAnalog = 20,
        OnInbox = 21,
        OnOptions = 22,
        OnIntel = 23,
        OnIntelLeft = 24,
        OnPressNextTab = 25,
        OnPressPrevTab = 26,
        OnCycleNext = 27,
        OnCyclePrev = 28,
        OnAnalogClockwise = 29,
        OnAnalogCounterClockwise = 30,
        OnPressAcceptHold = 32,
        OnPressCancelHold = 33,
        OnPressStartHold = 34,
        OnPressDpadUpHold = 35,
        OnPressDpadDownHold = 36,
        OnPressDpadLeftHold = 37,
        OnPressDpadRightHold = 38,
        OnPressUpHold = 39,
        OnPressDownHold = 40,
        OnPressLeftHold = 41,
        OnPressRightHold = 42,
        OnPressLeftAnalogHold = 43,
        OnPressRightAnalogHold = 44,
        OnInboxHold = 45,
        OnOptionsHold = 46,
        OnIntelHold = 47,
        OnPressNextTabHold = 48,
        OnPressPrevTabHold = 49,
        OnCycleNextHold = 50,
        OnCyclePrevHold = 51,
        OnAnalogClockwiseHold = 52,
        OnAnalogCounterClockwiseHold = 53,
        OnReleaseAccept = 55,
        OnReleaseCancel = 56,
        OnReleaseStart = 57,
        OnReleaseDpadUp = 58,
        OnReleaseDpadDown = 59,
        OnReleaseDpadLeft = 60,
        OnReleaseDpadRight = 61,
        OnReleaseUp = 62,
        OnReleaseDown = 63,
        OnReleaseLeft = 64,
        OnReleaseRight = 65,
        OnReleaseLeftAnalog = 66,
        OnReleaseRightAnalog = 67,
        OnReleaseInbox = 68,
        OnReleaseOptions = 69,
        OnReleaseIntel = 70,
        OnReleaseNextTab = 71,
        OnReleasePrevTab = 72,
        OnCycleNextRelease = 73,
        OnCyclePrevRelease = 74,
        OnPressAcceptHoldAndRelease = 77,
        OnPressCancelHoldAndRelease = 78,
        OnPressStartHoldAndRelease = 79,
        OnPressDpadUpHoldAndRelease = 80,
        OnPressDpadDownHoldAndRelease = 81,
        OnPressDpadLeftHoldAndRelease = 82,
        OnPressDpadRightHoldAndRelease = 83,
        OnPressUpHoldAndRelease = 84,
        OnPressDownHoldAndRelease = 85,
        OnPressLeftHoldAndRelease = 86,
        OnPressRightHoldAndRelease = 87,
        OnPressLeftAnalogHoldAndRelease = 88,
        OnPressRightAnalogHoldAndRelease = 89,
        OnInboxHoldAndRelease = 90,
        OnOptionsHoldAndRelease = 91,
        OnIntelHoldAndRelease = 92,
        OnPressNextTabHoldAndRelease = 93,
        OnPressPrevTabHoldAndRelease = 94,
        OnCycleNextHoldAndRelease = 95,
        OnCyclePrevHoldAndRelease = 96,
        OnMouseAcceptClick = 100,
        OnMouseCancelClick = 101,
        OnMouseMiddleClick = 102,
        OnMouseWheelYUp = 103,
        OnMouseWheelYDown = 104,
        OnMouseAcceptRelease = 106,
        OnMouseCancelRelease = 107,
        OnMouseMiddleRelease = 108,
        OnMouseOver = 111,
        OnMouseRangeOut = 112,
        OnMouseAcceptHold = 115,
        OnFocusOn = 1,
        OnFocusOff = 2,
        OnPageOn = 3,
        OnPageOff = 4,
        OnValueChanged = 5,
    }

    [RTTI.Serializable(0x727229DC15D8EB0C, GameType.DS)]
    public enum EMenuFunctionBindingVariable : int8
    {
        Index = 0,
        ParentIndex = 1,
        ChildCount = 2,
    }

    [RTTI.Serializable(0x9E8F878F0E4183A3, GameType.DS)]
    public enum EMenuInputFunction : int32
    {
        FUNCTION_UNSET = -1,
        FUNCTION_MOUSE_ACCEPT = 0,
        FUNCTION_MOUSE_CANCEL = 1,
        FUNCTION_MOUSE_MIDDLE = 2,
        FUNCTION_MOUSE_OVER = 3,
        FUNCTION_MOUSE_RANGE_OUT = 4,
        FUNCTION_MOUSE_SCROLL_UP = 5,
        FUNCTION_MOUSE_SCROLL_DOWN = 6,
        FUNCTION_MOUSE_ACCEPT_PAD = 7,
        FUNCTION_DPAD_NAV_UP = 10,
        FUNCTION_DPAD_NAV_DOWN = 11,
        FUNCTION_DPAD_NAV_LEFT = 12,
        FUNCTION_DPAD_NAV_RIGHT = 13,
        FUNCTION_NAV_UP = 14,
        FUNCTION_NAV_DOWN = 15,
        FUNCTION_NAV_LEFT = 16,
        FUNCTION_NAV_RIGHT = 17,
        FUNCTION_SCROLL_UP = 18,
        FUNCTION_SCROLL_DOWN = 19,
        FUNCTION_ACCEPT = 20,
        FUNCTION_OPEN_VKB = 21,
        FUNCTION_CANCEL = 22,
        FUNCTION_TAB_PREVIOUS = 23,
        FUNCTION_TAB_NEXT = 24,
        FUNCTION_CYCLE_PREVIOUS = 25,
        FUNCTION_CYCLE_NEXT = 26,
        FUNCTION_INBOX = 27,
        FUNCTION_MENU_OPTIONS = 28,
        FUNCTION_INGAME_OPTIONS = 29,
        FUNCTION_INGAME_INTEL = 30,
        FUNCTION_ANALOG_CLOCKWISE = 31,
        FUNCTION_ANALOG_COUNTERCLOCKWISE = 32,
        FUNCTION_ANALOG_LEFT = 34,
        FUNCTION_ANALOG_RIGHT = 33,
        FUNCTION_PHOTOMODE_OPEN = 35,
    }

    [RTTI.Serializable(0x66C8FA0112980274, GameType.DS)]
    public enum EMenuMovieAction : int8
    {
        Start = 0,
        StartFromLastKeyFrame = 1,
        Stop = 2,
    }

    [RTTI.Serializable(0xB446FC0F05884F9, GameType.DS)]
    public enum EMenuOrientation : int8
    {
        Unset = 0,
        Horizontal = 1,
        Vertical = 2,
    }

    [RTTI.Serializable(0xA1D7944E0CB4B9D0, GameType.DS)]
    public enum EMenuPrefabArrayPropertyMode : int32
    {
        Add_entries_to_the_existing_array = 0,
        Overwrite_entries_of_the_existing_array = 1,
    }

    [RTTI.Serializable(0x8B3F7A89047F440D, GameType.DS)]
    public enum EMeshEmitterSpawnOrder : int32
    {
        Point_order = 0,
        Random_order = 1,
    }

    [RTTI.Serializable(0xCA1F401917570681, GameType.DS)]
    public enum EMissionType : int32
    {
        campaign = 31,
        coop = 30,
    }

    [RTTI.Serializable(0xB3A0B97363002E13, GameType.DS)]
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

    [RTTI.Serializable(0xD8E960FD71C3A11D, GameType.DS)]
    public enum EModificationItemCategory : int8
    {
        Weapon = 0,
        Outfit = 1,
    }

    [RTTI.Serializable(0x1487F981AC47770C, GameType.DS)]
    public enum EModificationSocketActivationType : int8
    {
        Invalid = -1,
        Weapon = 0,
        Equipment = 1,
        Outfit = 2,
    }

    [RTTI.Serializable(0x30CB9FF9A4E7DC81, GameType.DS)]
    public enum EMountRequest : int8
    {
        Mount = 0,
        Dismount = 1,
    }

    [RTTI.Serializable(0x7764D72164C08F4F, GameType.DS)]
    public enum EMountState : int8
    {
        Unmounted = 3,
        Mounting = 0,
        Mounted = 1,
        Dismounting = 2,
    }

    [RTTI.Serializable(0xD51C0F39D428565B, GameType.DS)]
    public enum EMountedState : int32
    {
        Any = 0,
        Unmounted = 1,
        Mounted = 2,
    }

    [RTTI.Serializable(0x1989076CE5C06BAC, GameType.DS)]
    public enum EMouseButton : int8
    {
        Left = 0,
        Right = 1,
        Middle = 2,
        Button4 = 3,
        Button5 = 4,
        WheelXLeft = 5,
        WheelXRight = 6,
        WheelYUp = 7,
        WheelYDown = 8,
    }

    [RTTI.Serializable(0x2839A5DF088DABFD, GameType.DS)]
    public enum EMoveStanceChoice : int32
    {
        Fast = 0,
        Stealth = 1,
    }

    [RTTI.Serializable(0x913848C879A8AD3B, GameType.DS)]
    public enum EMovementStateGroundToAir : int8
    {
        On_Ground = 0,
        Taking_Off = 1,
        Landing = 3,
        Flying = 2,
    }

    [RTTI.Serializable(0x5498ACA490AF9595, GameType.DS)]
    public enum EMoverActionParentLinking : int32
    {
        DontChange = 0,
        AttachToActivator = 1,
        DetachFromParent = 2,
    }

    [RTTI.Serializable(0x6FCAD7DDEB17485F, GameType.DS)]
    public enum EMovieFadePurpose : int32
    {
        None = 0,
        Intro = 2,
        Outro = 3,
        Taboo = 1,
    }

    [RTTI.Serializable(0x9667CA356B15D4B7, GameType.DS)]
    public enum EMovieMemoryType : int32
    {
        Heap = 0,
        Post = 1,
    }

    [RTTI.Serializable(0x22DA4F3DC400014B, GameType.DS)]
    public enum EMsgAIAttackState : int8
    {
        Start = 0,
        Complete = 1,
        Abort = 2,
    }

    [RTTI.Serializable(0xB98DCF58C779B86, GameType.DS)]
    public enum EMsgMissionEventTimerMessageType : int32
    {
        Unknown = 0,
        CountStart = 1,
        CountEnd = 2,
        SplitEvent = 3,
    }

    [RTTI.Serializable(0x760BA9B8DAAA5D61, GameType.DS)]
    public enum EMsgSceneDSCountTimerMessageType : int32
    {
        Unknown = 0,
        CountStart = 1,
        CountPause = 2,
        CountEnd = 3,
        SplitEvent = 4,
    }

    [RTTI.Serializable(0xB28E1EFF5A53FDEA, GameType.DS)]
    public enum EMuleVoiceType : int32
    {
        StaminaDown = 0,
        PunchOrKickDown = 1,
        BolaGunDown = 2,
        ElectricDown = 3,
        FallAsleep = 4,
        LightDamage = 5,
        HeavyDamage = 6,
        HeavyDamageEndurePain = 7,
        KillDamage = 8,
        KickDamage = 9,
        PunchDamage = 10,
        BreathInSleep = 11,
        SnoringInSleep = 12,
        StruggleDuringRopeDown = 13,
        PainDuringDying = 14,
        CQCRopeDown = 15,
        SmokeCough = 16,
        DownInWater = 17,
        CoughInWater = 18,
        Slip = 19,
        NoticeJoy = 20,
        NoticeBaggage = 21,
        NoticeSomethingSurprise = 22,
        NoticeSomething = 23,
        NoticeTimefall = 24,
        NoticeFindNothing = 25,
        NoticeAgainstBT = 26,
        NoticeWakeupMule_KnockedOut = 27,
        NoticeWakeupMule_TiedUp = 28,
        LightAttack = 29,
        MediumAttack = 30,
        HeavyAttack = 31,
        Attempting = 32,
        AngryBattleCry = 33,
        FrQuestioningGrunt = 34,
        FrReceivingSomething = 35,
        FrDifficult = 36,
        FrDisappointed = 37,
        FrCallingA = 38,
        FrCallingB = 39,
        FrFearful = 40,
        FrHello_Near = 41,
        FrHello_Far = 42,
        BtVoidout = 43,
        OverTheFence = 44,
        ClimbingTheWalls = 45,
        ExtendArm = 46,
        ExtendArmLong = 47,
        YawnShort = 48,
        PickUpHeavyCargo = 49,
        SetDownHeavyCargo = 50,
        BreathWhenCarryingHeavyCargo_Run = 51,
        BreathWhenCarryingHeavyCargo_Walk = 52,
        RunningBreath_Run = 53,
        RunningBreath_Sprint = 54,
        ExhaustedRunBreath = 55,
        CanNotRunStart = 56,
        CanNotRunLoop = 57,
        CanNotRunEnd = 58,
        LightEffort = 59,
        MaxValue = 60,
    }

    [RTTI.Serializable(0x5D7628348BF5C547, GameType.DS)]
    public enum EMusicClipType : int8
    {
        OneShot = 0,
        Loop = 1,
    }

    [RTTI.Serializable(0xAE535EA94909B438, GameType.DS)]
    public enum EMusicJumpConditionType : int8
    {
        Always = 0,
        Random = 1,
        Graph = 2,
    }

    [RTTI.Serializable(0x2F2DA87877B2246D, GameType.DS)]
    public enum EMusicSyncMode : int8
    {
        Immediate = 0,
        Next_Beat = 1,
        Next_Bar = 2,
        Next_Marker = 3,
    }

    [RTTI.Serializable(0xA9CD3B8D74F4B30, GameType.DS)]
    public enum EMusicTransitionCurveType : int32
    {
        Linear = 1,
        Fast = 2,
        Slow = 3,
        Smooth = 4,
        Sharp = 5,
    }

    [RTTI.Serializable(0xFE3F700346F72175, GameType.DS)]
    public enum EMusicTransitionMode : int32
    {
        Seconds = 0,
        Beats = 1,
    }

    [RTTI.Serializable(0x39EA371C6D95B8C5, GameType.DS)]
    public enum EMusicTransitionUnit : int8
    {
        Seconds = 0,
        Beats = 1,
        Bars = 2,
    }

    [RTTI.Serializable(0xC0CF16FEFD9F8DBD, GameType.DS)]
    public enum ENameExposureType : int32
    {
        Never = 0,
        Always = 1,
        OnTarget = 2,
    }

    [RTTI.Serializable(0x38356D595DB33C40, GameType.DS)]
    public enum ENodeGraphComponentReplicationMode : int8
    {
        OnlyLocal = 0,
        OnlyRemote = 1,
        LocalAndRemote = 2,
    }

    [RTTI.Serializable(0x82889CC88EEFC5F1, GameType.DS)]
    public enum EOTGCAuthProvider : int8
    {
        identity = 0,
        psn = 1,
        steam = 2,
        epic = 3,
    }

    [RTTI.Serializable(0xAE930EDD28ABD044, GameType.DS)]
    public enum EObstacleShape : int32
    {
        Entity_Physics = 1,
        Entity_Physics_Bounding_Box = 2,
        Custom_Box = 3,
        Custom_Capsule = 4,
    }

    [RTTI.Serializable(0x7205DA5456ECAABA, GameType.DS)]
    public enum EObstacleType : int32
    {
        Normal = 1,
        Ignore = 0,
        Soft = 2,
        Hard = 3,
    }

    [RTTI.Serializable(0xE23B0CF8E27DF8DC, GameType.DS)]
    public enum EOdradekSurveyShapeMode : int32
    {
        Sphere = 0,
        Fan = 1,
        Spherical = 2,
    }

    [RTTI.Serializable(0x604598E99B42EA29, GameType.DS)]
    public enum EOpacityMode : int32
    {
        _0 = 0,
        inherit = 1,
        ignore = 2,
    }

    [RTTI.Serializable(0x92A1F61F578580EE, GameType.DS)]
    public enum EPAINT_Mode : int32
    {
        Mode_None = -1,
        Mode_Sphere = 0,
        Mode_Sphere_undir = 1,
        Mode_Plane = 2,
        Mode_All = 3,
    }

    [RTTI.Serializable(0x621396B8E31D925B, GameType.DS)]
    public enum EPAINT_Operation : int32
    {
        Operation_Add = 0,
        Operation_Sub = 1,
        Operation_Mul = 2,
    }

    [RTTI.Serializable(0x10D64D59B8D975AE, GameType.DS)]
    public enum EPBDConstraintDescType : int32
    {
        Distance = 1,
        DistanceLRA = 6,
        Bend = 7,
    }

    [RTTI.Serializable(0x59AB830AD2E4FAF6, GameType.DS)]
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

    [RTTI.Serializable(0xBDA1576BDD21EF20, GameType.DS)]
    public enum EPS4ProRenderMode : int32
    {
        PS4ProRenderModeHighResolution = 0,
        PS4ProRenderModeHighFramerate = 1,
    }

    [RTTI.Serializable(0x77CA907ECD86DA62, GameType.DS)]
    public enum EPanelScrollType : int32
    {
        Unset = -1,
        Horizontal = 0,
        Vertical = 1,
    }

    [RTTI.Serializable(0x97FC6B7B5F645216, GameType.DS)]
    public enum EParentObjectiveVisibilityLogic : int32
    {
        AlwaysShow = 0,
        ShowWhenSubObjectivesVisible = 1,
        ShowWhenSubObjectivesHidden = 2,
    }

    [RTTI.Serializable(0x1704CA8EACDB8CD3, GameType.DS)]
    public enum EParkourTransitionLimitAxis : int8
    {
        X = 0,
        Y = 1,
        Z = 2,
    }

    [RTTI.Serializable(0xF2B5133B41F41A92, GameType.DS)]
    public enum EParkourTransitionLimitSimpleShape : int8
    {
        Ellipse = 0,
        Box = 1,
    }

    [RTTI.Serializable(0xE9A326FB734453FF, GameType.DS)]
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

    [RTTI.Serializable(0xB46B16BF6FE86B1D, GameType.DS)]
    public enum EParticleCollisionMode : int32
    {
        RaycastCollision = 0,
        ScreenSpaceCollision = 1,
    }

    [RTTI.Serializable(0xDED6B857EDFC779D, GameType.DS)]
    public enum EParticleControlledAttributeSource : int32
    {
        None = 0,
        Lifetime = 1,
        Lifespan = 2,
        Velocity = 3,
        Random = 4,
        Effecttime = 5,
    }

    [RTTI.Serializable(0xE8A9011ECBA922D4, GameType.DS)]
    public enum EParticleEmitRateUnits : int32
    {
        ParticlesPerSecond = 0,
        ParticlesPerMeter = 1,
    }

    [RTTI.Serializable(0x69BC2CA5DF064AF7, GameType.DS)]
    public enum EParticleFadeMode : int32
    {
        No_Fading = 0,
        Per_Particle_Fading = 1,
    }

    [RTTI.Serializable(0xC84A754C090715AF, GameType.DS)]
    public enum EParticlePivotAligment : int32
    {
        Top = 0,
        Center = 1,
        Bottom = 2,
    }

    [RTTI.Serializable(0xA06BE3DBD4FEB043, GameType.DS)]
    public enum EParticleShape : int32
    {
        FlatQuad = 0,
        TentedQuad = 1,
        PolyTrail = 2,
        Octagonal = 3,
        StretchStrip = 4,
    }

    [RTTI.Serializable(0xFBA2808F9C8754F6, GameType.DS)]
    public enum EParticleSubTexAnimationSrc : int32
    {
        ParticleAge = 0,
        ParticleLifetime = 1,
        ParticleVelocity = 2,
    }

    [RTTI.Serializable(0x4DF4CCA30383B526, GameType.DS)]
    public enum EParticleSystemUpdateMode : int32
    {
        Always = 0,
        WhenVisible = 1,
    }

    [RTTI.Serializable(0xC98E6D7AB57F3F6D, GameType.DS)]
    public enum EPathMode : int32
    {
        Time = 0,
        Distance = 1,
    }

    [RTTI.Serializable(0x6CFF437E96A6E0D3, GameType.DS)]
    public enum EPerkAbility : int8
    {
        HorseCall = 0,
        LureEnemy = 1,
        Invalid = -1,
    }

    [RTTI.Serializable(0x3F5FE3137F79609F, GameType.DS)]
    public enum EPerkPointGainReason : int32
    {
        Initial = 0,
        Restore = 1,
        LevelUp = 2,
        Quest = 3,
        Script = 4,
        Debug = 5,
    }

    [RTTI.Serializable(0xADA5BA875170BBAD, GameType.DS)]
    public enum EPhotoModeValueType : int8
    {
        Unknown = 0,
        Bool = 1,
        Int = 2,
        Float = 3,
        Enum = 4,
    }

    [RTTI.Serializable(0x5332C2629259DD3, GameType.DS)]
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
        Player_and_Bullet_blocker = 15,
        vs_Default_Character_Blocker = 16,
        Vehicle = 17,
        Vehicle_stopper = 18,
        Humanoid_movement_helper = 19,
        Projectile = 20,
        Player_Damage_Blocker = 21,
        Enemy_Damage_Blocker = 22,
        BT_Damage_Blocker = 23,
        Neutral_Damage_Blocker = 24,
        vs_Humanoids = 25,
        Camera_through = 26,
        vs_Camera_blocker = 27,
        push = 28,
        vs_not_enemy = 29,
        Dynamic_HQ_but_humanoid = 30,
        Proxy_player = 31,
        vs_Eye_blocker = 32,
        DS_Dynamic_Small_Gimmick = 33,
        DS_Player_Movement_Blocker = 34,
        DS_Strand_object = 35,
        Blocks_vision = 36,
        Player_Ragdoll = 37,
        Ragdoll_Damage_Blocker = 38,
        Player_and_Bullet_blocker_no_navi = 39,
        Layer_without_no_navi = 40,
        Player_and_Bullet_blocker_no_solid_navi = 41,
        Layer_without_no_solid_navi = 42,
        Layer_with_no_solid_navi_only = 43,
        Static_But_Humanoid = 44,
        Camera_blocker = 45,
        Particles_Collision = 46,
        Ray_vs_Static = 47,
        Vehicle_wheel_raycast = 48,
        Camera_Obstruction = 58,
        Navigation_Mesh = 59,
        Vault_Query = 60,
        Cliff_Floor = 61,
        Deep_Water_Surface = 62,
        VS_Structure_Blocker = 63,
        Navigation_Mesh_Hard_Obstacle = 64,
        Semi_Static = 65,
        Camera_Collision = 66,
        DS_Dynamic_Recoil = 67,
        DS_vs_Static_only = 68,
        Static_but_Navigation_Mesh = 69,
        No_Vault_Action = 70,
        Leg_IK_raycast = 71,
        Lightbake_Visibility = 72,
        DS_Rope_To_Check = 73,
        Enemy_only_blocker = 74,
        Road_dont_fall = 75,
        Cliff_Wall = 76,
        Toxic_Gas_Zone = 77,
        DS_Eye_blocker = 78,
        vs_Enemy_Character_Blocker = 79,
        VS_Damage_Blocker = 80,
        Dont_fall = 81,
        DS_Baggage = 82,
        vs_Enemy_Character_Blocker_Allow_Fall = 83,
        DS_Dynamic_Heavy_Gimmick = 84,
        Chiral_Wall = 85,
        Player_blocker_without_vehicle = 86,
        Player_and_Bullet_blocker_without_vehicle = 87,
        Grass = 88,
        Construction_Region = 89,
        vs_Construction_Region = 90,
        Enemy_and_Vehicle_blocker = 91,
        DS_Ladder = 92,
        DS_Construction_Checker = 93,
        vs_Dynamic_Objects = 94,
        No_Vault_Action_Player_Bullet_Blocker = 95,
        DS_Player_Leg_IK_Raycast = 96,
        Humanoid_and_Vehicle_blocker = 97,
        vs_Dont_fall = 98,
        Boss_Only_Blocker = 99,
        vs_Boss_Only_Blocker = 100,
        Sound_Pole = 101,
        vs_BulletBlocker_and_SoundPole = 102,
        Projectile_Baggage = 103,
        Chiral_Wall_Warrior = 104,
        BT_Handprint = 105,
        DS_Dynamic_Middle_Gimmick = 106,
        vs_bullet_blocker_without_water = 107,
        vs_bullet_blocker_without_No_Vault_Action = 108,
        DS_Ladder_Checker = 109,
        weapon_case_debris = 110,
        DS_Dynamic_Eye_blocker = 111,
        vs_Chiral_Wall = 112,
        Projectile_for_ammo_cartridge = 113,
        Static_Debug = 114,
        Dynamic_Debug = 115,
        Debug_Draw = 116,
        Density_Debug = 117,
        Collision_Check_Tool = 118,
    }

    [RTTI.Serializable(0xDCF243A8FB8CD72E, GameType.DS)]
    public enum EPhysicsMotionType : int32
    {
        Dynamic = 1,
        Keyframed = 2,
        Static = 3,
    }

    [RTTI.Serializable(0x5C379527ED6288E4, GameType.DS)]
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
        Vehicle = 9,
    }

    [RTTI.Serializable(0xAC39124479CAFF7D, GameType.DS)]
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
        CapsuleX = 12,
        CapsuleY = 13,
    }

    [RTTI.Serializable(0xA60DEC15882A341F, GameType.DS)]
    public enum EPickUpAnimationWieldDirective : int8
    {
        DoNothing = 0,
        StowWeapon = 1,
        SwitchToMeleeWeaponImmidiately = 2,
    }

    [RTTI.Serializable(0x2CB3675607AF1068, GameType.DS)]
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

    [RTTI.Serializable(0x2407E8007C424BC7, GameType.DS)]
    public enum EPlacementChunkSizeSetting : int32
    {
        Auto = 0,
        Small = 1,
        Medium = 2,
        Large = 3,
    }

    [RTTI.Serializable(0xCEEDF535B9633D81, GameType.DS)]
    public enum EPlacementRotationType : int32
    {
        AxisAligned = 0,
        TowardsSlope = 1,
        Full = 2,
    }

    [RTTI.Serializable(0xC2979D608C1E2B5A, GameType.DS)]
    public enum EPlacementUsageMask : int32
    {
        ObserverOnly = 1,
        AreaOnly = 2,
        All = 3,
    }

    [RTTI.Serializable(0xD81B40A2B91FB08D, GameType.DS)]
    public enum EPlatform : int32
    {
        PC = 0,
        PINK = 1,
        PS5 = 2,
        Linux = 3,
    }

    [RTTI.Serializable(0xEF7AE43096A9D383, GameType.DS)]
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

    [RTTI.Serializable(0x61150FD8147D2B8C, GameType.DS)]
    public enum EPlayerHealthSettings : int32
    {
        Low = 0,
        Normal = 1,
        High = 2,
    }

    [RTTI.Serializable(0xE22565490DB643BB, GameType.DS)]
    public enum EPlayerNumber : int32
    {
        None = -1,
        _0 = 0,
        _1 = 1,
        _2 = 2,
        _3 = 3,
    }

    [RTTI.Serializable(0xDA678F9AB9CBEE69, GameType.DS)]
    public enum EPlaylistFilterOperation : int32
    {
        EQUALS = 0,
        NOT_EQUALS = 1,
        CONTAINS_ALL = 2,
        NOT_CONTAINS_ALL = 3,
        CONTAINS_ANY = 4,
        CONTAINS_NONE = 5,
    }

    [RTTI.Serializable(0x85175B948B069BBA, GameType.DS)]
    public enum EPointOfAimRotation : int32
    {
        Camera = 0,
        Chest = 1,
        Position = 2,
    }

    [RTTI.Serializable(0xA55A2A1531595EDB, GameType.DS)]
    public enum EPositionAssessment : int32
    {
        invalid = -1,
        observed_exact = 0,
        deduced_exact = 1,
        deduced_rough = 2,
        deduced_unknown = 3,
        confirmed_lost = 4,
    }

    [RTTI.Serializable(0x3B32A1820C6E08C7, GameType.DS)]
    public enum EPostProcessBlendMode : int32
    {
        Lerp = 0,
        Add = 1,
    }

    [RTTI.Serializable(0x3A5CEF69D6CEFCC7, GameType.DS)]
    public enum EPreviewProjectileCreateMode : int32
    {
        Wielding = 0,
        Charging = 1,
    }

    [RTTI.Serializable(0x536FF2B448C227A8, GameType.DS)]
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

    [RTTI.Serializable(0x1C7846DD2E4C739F, GameType.DS)]
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
        Perforce_error = 10,
    }

    [RTTI.Serializable(0xB9098BBD2DA1DF39, GameType.DS)]
    public enum EProfileUpdateStatus : int32
    {
        SUCCESS = 0,
        ALREADY_APPLIED = 1,
        TRY_AGAIN = 2,
        INVALID_UPDATE = 3,
    }

    [RTTI.Serializable(0x8E32C40558E36E3E, GameType.DS)]
    public enum EProgramType : int32
    {
        VertexProgram = 2,
        GeometryProgram = 1,
        PixelProgram = 3,
        ComputeProgram = 0,
    }

    [RTTI.Serializable(0x2D12E73211551897, GameType.DS)]
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

    [RTTI.Serializable(0x43620D9A4C9F2E30, GameType.DS)]
    public enum EProjectionMode : int32
    {
        Perspective = 0,
        Orthogonal = 1,
    }

    [RTTI.Serializable(0xE79A08F9FE511B35, GameType.DS)]
    public enum EQuestRunState : int8
    {
        Running = 0,
        Paused = 1,
        UniqueBlocked = 2,
        Cooldown = 4,
    }

    [RTTI.Serializable(0x8834880540C5024, GameType.DS)]
    public enum EQuestSectionCompletionType : int32
    {
        Any = 0,
        All = 1,
    }

    [RTTI.Serializable(0x58554487B60BB70B, GameType.DS)]
    public enum EQuestSectionDependencyType : int32
    {
        And = 0,
        Or = 1,
    }

    [RTTI.Serializable(0x2C2AB5EB7B811D0, GameType.DS)]
    public enum EQuestSectionState : int32
    {
        Unavailable = 0,
        Available = 1,
        Completed = 2,
        Blocked = 3,
    }

    [RTTI.Serializable(0xA9DC92B16858930A, GameType.DS)]
    public enum EQuestSectionType : int32
    {
        Start = 0,
        Progress = 1,
        Success = 2,
        Fail = 3,
    }

    [RTTI.Serializable(0xF108F2A42224D0DD, GameType.DS)]
    public enum EQuestState : int32
    {
        Unavailable = 0,
        Available = 1,
        InProgress = 2,
        Succeeded = 3,
        Failed = 4,
    }

    [RTTI.Serializable(0x3298C3B739F5F4D, GameType.DS)]
    public enum ERandomShaderVariableType : int8
    {
        DontRandomize = 0,
        SingleRandomValueForAllParts = 1,
        RandomValuePerPart = 2,
    }

    [RTTI.Serializable(0xE38BC840DD2431F8, GameType.DS)]
    public enum EReactionEndType : int32
    {
        Finish = 0,
        Skip = 1,
        Decay = 2,
        Abort = 3,
    }

    [RTTI.Serializable(0x33996D681A67A36F, GameType.DS)]
    public enum EReactionPassThroughType : int32
    {
        Stop_Here = 0,
        Skip_and_Continue = 1,
        Play_and_Continue = 2,
    }

    [RTTI.Serializable(0xF7FA768C0CCBAE1D, GameType.DS)]
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
        Test = 10,
    }

    [RTTI.Serializable(0xFA671FD95C527BA, GameType.DS)]
    public enum ERenderDataHintDataType : int8
    {
        FrameBased = 0,
        GridBased = 1,
        AreaBased = 2,
        Invalid = 3,
    }

    [RTTI.Serializable(0x85042220834261B5, GameType.DS)]
    public enum ERenderDataStreamingObjectBoostMode : int8
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Immediate = 4,
    }

    [RTTI.Serializable(0x566A9A507303C3D7, GameType.DS)]
    public enum ERenderEffectAllocationMode : int8
    {
        Private = 0,
        Cached = 1,
    }

    [RTTI.Serializable(0x624C2EAC411306B9, GameType.DS)]
    public enum ERenderEffectType : int32
    {
        Object_render_effect = 0,
        Spotlight_render_effect = 1,
        Omnilight_render_effect = 2,
        Sunlight_render_effect = 3,
    }

    [RTTI.Serializable(0x6387342F41783564, GameType.DS)]
    public enum ERenderPlatform : int8
    {
        DX = 0,
        DX12 = 1,
        PINK = 2,
        PS5 = 3,
        Vulkan = 4,
        Invalid = 5,
    }

    [RTTI.Serializable(0x464DA288B9E39D4F, GameType.DS)]
    public enum ERenderTechniqueSetType : int32
    {
        Invalid_rendering_techniques = -1,
        Normal_rendering_techniques = 0,
        Instanced_techniques = 1,
    }

    [RTTI.Serializable(0x22531E1B22769FCC, GameType.DS)]
    public enum ERenderTechniqueType : int32
    {
        Invalid = -1,
        Direct = 0,
        Unlit = 1,
        DepthOnly = 2,
        MaskedDepthOnly = 3,
        Deferred = 4,
        DeferredSimplified = 12,
        DeferredEmissive = 5,
        DeferredTransAcc = 6,
        DeferredTrans = 7,
        CustomDeferredBackground = 8,
        CustomDeferredNormalRead = 10,
        CustomDeferredDepthWrite = 11,
        CustomDeferred = 9,
        HalfDepthOnly = 13,
        LightSampling = 14,
        CustomForward = 15,
        Transparency = 16,
        ForwardBackground = 17,
        ForwardWaterFromBelow = 18,
        ForwardHalfRes = 19,
        ForwardQuarterRes = 20,
        ForwardMotionVectors = 21,
        ForwardForeground = 22,
        VolumeLightAmount = 23,
        Shadowmap = 24,
    }

    [RTTI.Serializable(0x99538AF4793441A8, GameType.DS)]
    public enum ERenderZoneFadeRegion : int32
    {
        Inwards = 0,
        Outwards = 1,
    }

    [RTTI.Serializable(0xE060ECC7704CD7DC, GameType.DS)]
    public enum ERequiredJumpMovementState : int8
    {
        Unrestricted = 0,
        Moving = 1,
        StandingStill = 2,
    }

    [RTTI.Serializable(0x260D751B3FCB588B, GameType.DS)]
    public enum ERoadBakeDataMode : int32
    {
        None = 0,
        Height = 1,
        Topo_Roads = 2,
        HeightAndTopoRoads = 3,
    }

    [RTTI.Serializable(0xBE77D61B072EEDC1, GameType.DS)]
    public enum ERoadNodeProfileType : int32
    {
        Path = 0,
        Trail = 1,
        Trail_Snow = 2,
        Road = 3,
    }

    [RTTI.Serializable(0xFFB895E2CC939A6B, GameType.DS)]
    public enum ERoadNodeSnapMode : int32
    {
        Snap_To_Terrain_Height = 0,
        Use_Road_Height = 1,
    }

    [RTTI.Serializable(0xC8F130894EA1BF7, GameType.DS)]
    public enum ERootBoneMode : int32
    {
        Relative = 0,
        Absolute = 1,
        None = 2,
    }

    [RTTI.Serializable(0xE5D2C990AEE8D76C, GameType.DS)]
    public enum ERopeMode : int32
    {
        Anchor = 0,
        Tripwire = 1,
        Climbable = 2,
    }

    [RTTI.Serializable(0xE78A8DDC8C062290, GameType.DS)]
    public enum ERotationOrder : int32
    {
        ZYX = 0,
        YZX = 1,
        ZXY = 2,
        XZY = 3,
        YXZ = 4,
        XYZ = 5,
    }

    [RTTI.Serializable(0x14C676E7C6F65CDF, GameType.DS)]
    public enum ESHVOLUME_LODLevel : int32
    {
        Level0__4m_ = 0,
        Level1__8m_ = 1,
        Level2__16m_ = 2,
        Level3__32m_ = 3,
        Level4__64m_ = 4,
        Level5__128m_ = 5,
        Level6__256m_ = 6,
        Level7__512m_ = 7,
    }

    [RTTI.Serializable(0x11B45BBA6D3DDF0, GameType.DS)]
    public enum ESRTCreationMode : int8
    {
        SplitPerProgramType = 0,
        Merged = 1,
        Inline = 2,
        Invalid = 3,
    }

    [RTTI.Serializable(0x428C33D804ECE942, GameType.DS)]
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
        unorm_float = 9,
        unorm_float2 = 10,
        unorm_float3 = 11,
        unorm_float4 = 12,
        snorm_float = 13,
        snorm_float2 = 14,
        snorm_float3 = 15,
        snorm_float4 = 16,
        _int = 17,
        int2 = 18,
        int3 = 19,
        int4 = 20,
        _uint = 21,
        uint2 = 22,
        uint3 = 23,
        uint4 = 24,
        float2x3 = 25,
        float3x4 = 26,
        float4x4 = 27,
        subset = 28,
        structured = 29,
    }

    [RTTI.Serializable(0x3782B572CDC5BA66, GameType.DS)]
    public enum ESRTElementType : int8
    {
        Unknown = 0,
        Constant = 1,
        Texture1D = 2,
        Texture2D = 3,
        Texture3D = 4,
        TextureCube = 5,
        Texture2DArray = 6,
        Texture2DList = 7,
        TextureIrradianceVolume = 8,
        RWTexture2D = 9,
        RWTexture2DArray = 10,
        RWTexture3D = 11,
        Sampler = 12,
        ShadowSampler = 13,
        DataBuffer = 14,
        StructuredBuffer = 15,
        RWDataBuffer = 16,
        RWStructuredBuffer = 17,
        RWTextureCube = 18,
        RayTraceBVH = 19,
    }

    [RTTI.Serializable(0xA08CF04A2320CB13, GameType.DS)]
    public enum ESRTStorageMode : int8
    {
        ShaderInstance = 0,
        Scratch = 1,
    }

    [RTTI.Serializable(0xCECF30CE03BF586B, GameType.DS)]
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

    [RTTI.Serializable(0xC2A7C7999B3B4EE1, GameType.DS)]
    public enum ESaveGameRestoreReason : int8
    {
        Invalid = 0,
        Manual = 1,
        Continue = 2,
        MissionFailed = 3,
        Debug = 4,
        AllDream = 5,
        AllDream_GasFaint = 6,
        AllDream_NoDemo = 7,
        BlackSam = 8,
        BlackSam_Annihilation = 9,
    }

    [RTTI.Serializable(0xF80828285E2E8E71, GameType.DS)]
    public enum ESaveGameSlot : int8
    {
        Memory = -2,
        Auto = -1,
        Slot0 = 0,
        Slot1 = 1,
        Slot2 = 2,
        Slot3 = 3,
        Slot4 = 4,
        Slot5 = 5,
        Slot6 = 6,
        Slot7 = 7,
        Slot8 = 8,
        Slot9 = 9,
        Slot10 = 10,
        Slot11 = 11,
        Slot12 = 12,
        Slot13 = 13,
        Slot14 = 14,
        Slot15 = 15,
        Slot16 = 16,
        Slot17 = 17,
        Slot18 = 18,
        Slot19 = 19,
        Slot20 = 20,
        Slot21 = 21,
        Slot22 = 22,
    }

    [RTTI.Serializable(0x293ABDE694DE61FE, GameType.DS)]
    public enum ESaveGameType : int8
    {
        Manual = 1,
        Quick = 2,
        Automatic = 4,
        All = 15,
    }

    [RTTI.Serializable(0xDA58E669A1C8B5E7, GameType.DS)]
    public enum EScenarioInstancingRule : int32
    {
        SingleInstance = 0,
        MultipleInstances = 1,
    }

    [RTTI.Serializable(0xD8CDB8376AF31728, GameType.DS)]
    public enum ESceneActivationTriggerType : int8
    {
        ActivationTrigger = 0,
        DeactivationTrigger = 1,
    }

    [RTTI.Serializable(0xF17B0B3029B5B102, GameType.DS)]
    public enum ESceneActivationType : int32
    {
        Normal = 0,
        Large = 1,
        OwnedByParent = 2,
        Global = 3,
    }

    [RTTI.Serializable(0xFA44A5F205BC780E, GameType.DS)]
    public enum ESceneForcedActiveState : int8
    {
        NoForcedState = 0,
        ForcedActive = 1,
        ForcedInactive = 2,
    }

    [RTTI.Serializable(0x1292E222E74AC11A, GameType.DS)]
    public enum ESelectByFactContext : int8
    {
        Default = 0,
        Global = 1,
        Player = 2,
        Parent = 3,
    }

    [RTTI.Serializable(0x61E51DC75945F0B5, GameType.DS)]
    public enum ESelectByPropertyContext : int8
    {
        Default = 0,
        Player = 1,
        Parent = 2,
    }

    [RTTI.Serializable(0x2D2754F564ECE415, GameType.DS)]
    public enum ESelfDamage : int32
    {
        Yes = 1,
        No = 2,
        All = 3,
    }

    [RTTI.Serializable(0x22D6EBE2514387AF, GameType.DS)]
    public enum ESelfShadowMode : int32
    {
        None = 0,
        Fake = 1,
        Occlusion = 2,
    }

    [RTTI.Serializable(0xA647851A5C81EE3, GameType.DS)]
    public enum ESentenceDelivery : int8
    {
        on_actor = 1,
        radio = 2,
    }

    [RTTI.Serializable(0x83F51A3458088ABB, GameType.DS)]
    public enum ESentenceGroupType : int32
    {
        Normal = 0,
        OneOfRandom = 1,
        OneOfInOrder = 2,
    }

    [RTTI.Serializable(0x8704213DE5E685B2, GameType.DS)]
    public enum ESequenceCameraInterpMode : int8
    {
        Default = 0,
        KeepPosition = 1,
    }

    [RTTI.Serializable(0x3BCC1C9DC55FDBB8, GameType.DS)]
    public enum ESequenceCameraTransitionFunction : int8
    {
        TransitionLinear = 0,
        TransitionSmoothStep = 1,
    }

    [RTTI.Serializable(0xDE77A7BE96EEAED1, GameType.DS)]
    public enum ESequenceFactContextType : int8
    {
        Global = 0,
        Scene = 1,
        Actor = 2,
        Player = 3,
    }

    [RTTI.Serializable(0xE24318DA890841A8, GameType.DS)]
    public enum ESequenceHideBehavior : int32
    {
        Hide = 0,
        Remove = 1,
    }

    [RTTI.Serializable(0x9EE096ACC615438F, GameType.DS)]
    public enum ESequenceLoopMode : int8
    {
        Off = 0,
        Looping = 1,
    }

    [RTTI.Serializable(0xA473FDAAB2501982, GameType.DS)]
    public enum ESequenceNetworkBranchSelectionMode : int32
    {
        First = 0,
        Ordered = 1,
        Random = 2,
        Random_Unique = 3,
    }

    [RTTI.Serializable(0x80BF23ED04D2C150, GameType.DS)]
    public enum ESequenceNetworkFactContextType : int32
    {
        Global = 0,
        Local = 1,
        Scene = 2,
        Player = 3,
    }

    [RTTI.Serializable(0x6AE3CAE3A25932A2, GameType.DS)]
    public enum ESequenceNetworkTransitionSourceType : int8
    {
        None = 0,
        Any = 1,
        DefaultNext = 2,
        DefaultInterrupt = 3,
        InterruptHandler = 4,
        PlayerChoice = 5,
    }

    [RTTI.Serializable(0x936E44BF139D4128, GameType.DS)]
    public enum ESequenceNetworkTransitionTargetType : int8
    {
        None = 0,
        Any = 1,
        SequenceNode = 2,
    }

    [RTTI.Serializable(0x9D930C9BC78F2FB, GameType.DS)]
    public enum EServerType : int8
    {
        Static = 0,
        Dynamic = 1,
    }

    [RTTI.Serializable(0x90D1135E676F8CB5, GameType.DS)]
    public enum EShaderInstancingMode : int8
    {
        None = 0,
        MaterialInstancing = 1,
        OnTheFly = 2,
        Invalid = 3,
    }

    [RTTI.Serializable(0x3ABE71444D62EDB1, GameType.DS)]
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
        Int1 = 17,
        Int2 = 18,
        Int3 = 19,
        Int4 = 20,
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
        ConstUint1 = 105,
        ConstUint2 = 106,
        ConstUint3 = 107,
        ConstUint4 = 108,
        ConstInt1 = 113,
        ConstInt2 = 114,
        ConstInt3 = 115,
        ConstInt4 = 116,
        InstanceDataOffsetFloat1 = 129,
        InstanceDataOffsetFloat2 = 130,
        InstanceDataOffsetFloat3 = 131,
        InstanceDataOffsetFloat4 = 132,
    }

    [RTTI.Serializable(0x5A2AC6027CA00F1E, GameType.DS)]
    public enum EShadowBiasMode : int32
    {
        Multiplier = 0,
        AbsoluteBias = 1,
    }

    [RTTI.Serializable(0x4D351A964A2BE568, GameType.DS)]
    public enum EShadowCull : int32
    {
        Off = 0,
        CullFrontfaces = 2,
        CullBackfaces = 1,
    }

    [RTTI.Serializable(0x27118000B1CD9D7, GameType.DS)]
    public enum EShadowmapCacheForStaticGeometry : int32
    {
        No_cache_for_static_geometry = 0,
        Use_cache_for_static_geometry = 1,
        Use_cache_for_static_geometry__dynamic_geometry_ignored = 2,
        Map_size_varies_with_distance__cache_used_if___256 = 3,
        Map_size_varies_with_distance__cache_used_if___128 = 4,
    }

    [RTTI.Serializable(0x687A1F9B4157F045, GameType.DS)]
    public enum EShapeCurveSource : int8
    {
        ShapeOrigin = 0,
        CoreLine = 1,
    }

    [RTTI.Serializable(0xE3EDC2104D98A8A4, GameType.DS)]
    public enum EShowArcType : int8
    {
        Firing = 0,
        Aiming = 1,
        AimingNotFire = 2,
        WeaponIsActive = 3,
    }

    [RTTI.Serializable(0xC1FB87DB405534FE, GameType.DS)]
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

    [RTTI.Serializable(0x8DBFE39A12E9B721, GameType.DS)]
    public enum ESkinningDeformerType : int32
    {
        DeformPosAndNormals = 0,
        DeformPosAndComputeNormals = 1,
    }

    [RTTI.Serializable(0x44A9655D71FF03D7, GameType.DS)]
    public enum ESkipBehavior : int32
    {
        EndOfSequence = 0,
        EndOfEvent = 1,
        NotSkippable = 2,
    }

    [RTTI.Serializable(0x9EBBCE56C58EB3B9, GameType.DS)]
    public enum ESkipFade : int8
    {
        Default = 0,
        Black = 1,
        White = 2,
        None = 3,
        Wait = 4,
    }

    [RTTI.Serializable(0x8265321AF4BE7261, GameType.DS)]
    public enum ESkipLocationType : int32
    {
        Invalid = -1,
        Start = 0,
        Intro = 1,
        Interlude = 2,
    }

    [RTTI.Serializable(0x44D26B29DA316E63, GameType.DS)]
    public enum ESortMode : int32
    {
        FrontToBack = 1,
        BackToFront = 2,
        Off = 0,
    }

    [RTTI.Serializable(0xEB72A8208EDBF65, GameType.DS)]
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

    [RTTI.Serializable(0xC27F1E066FB0BE16, GameType.DS)]
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

    [RTTI.Serializable(0x8A5F355C69433DDD, GameType.DS)]
    public enum ESoundGroupType : int8
    {
        Sound_Effect = 0,
        Dialogue = 1,
        Music = 2,
        Music__Amadeus_ = 3,
    }

    [RTTI.Serializable(0xF90F920DBE1A9465, GameType.DS)]
    public enum ESoundInstanceLimitMode : int32
    {
        Off = 0,
        Stop_Oldest = 1,
        Stop_Softest = 2,
        Reject_New = 3,
    }

    [RTTI.Serializable(0x572B24B2C6598F79, GameType.DS)]
    public enum ESoundShape : int32
    {
        Sphere = 0,
        Box = 1,
        Cone = 2,
        Capsule = 3,
    }

    [RTTI.Serializable(0xFB5538E36B6067D8, GameType.DS)]
    public enum ESoundZoneShapeType : int32
    {
        Sphere = 0,
        Box = 1,
        Cone = 2,
        Capsule = 3,
    }

    [RTTI.Serializable(0x71C65662CB115AF3, GameType.DS)]
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

    [RTTI.Serializable(0xFE50278B8CC49C33, GameType.DS)]
    public enum ESpringAlignment : int8
    {
        ParentX = 0,
        ParentY = 1,
        ParentZ = 2,
        WorldX = 3,
        WorldY = 4,
        WorldZ = 5,
    }

    [RTTI.Serializable(0xE7615BD21114239D, GameType.DS)]
    public enum ESpringEvalSpace : int8
    {
        Off = 0,
        World = 1,
        Parent = 2,
        Local = 3,
    }

    [RTTI.Serializable(0x85FFED09F19E66C7, GameType.DS)]
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

    [RTTI.Serializable(0x1B63395DBF799FCA, GameType.DS)]
    public enum EStaminaType : int32
    {
        STAMINA = 0,
        ELECTRICITY = 1,
        BREATH = 2,
        DODGE = 3,
    }

    [RTTI.Serializable(0xE4BF315FA0FD26A8, GameType.DS)]
    public enum EStance : int32
    {
        INVALID = -1,
        STANDING = 0,
        CROUCHING = 1,
        LOWCROUCHING = 2,
    }

    [RTTI.Serializable(0x9538E1D34D32C2A5, GameType.DS)]
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

    [RTTI.Serializable(0x1372E3AF4B27E78B, GameType.DS)]
    public enum EStick : int32
    {
        Invalid = -1,
        Left = 0,
        Right = 1,
    }

    [RTTI.Serializable(0xBC3F79E15A476C2F, GameType.DS)]
    public enum EStickFunction : int32
    {
        Invalid = -1,
        Move = 0,
        Look = 1,
        InventorySelection = 2,
        DialogueChoice = 3,
        Zoom = 4,
    }

    [RTTI.Serializable(0x6B8C7B2C6BC3E80E, GameType.DS)]
    public enum EStreamingLODLevel : int8
    {
        SuperLow = 0,
        Low = 1,
        Medium = 2,
        High = 3,
    }

    [RTTI.Serializable(0x38D78C91B438CEF8, GameType.DS)]
    public enum EStreamingRefPriority : int8
    {
        None = 0,
        Lowest = 1,
        Lower = 2,
        Low = 3,
        BelowNormal = 4,
        Normal = 5,
        AboveNormal = 6,
        High = 7,
        Higher = 8,
        Highest = 9,
    }

    [RTTI.Serializable(0x45F600D02EB1567E, GameType.DS)]
    public enum ESubtitlePosition : int32
    {
        Bottom = 0,
        Top = 1,
    }

    [RTTI.Serializable(0xD8E4C44AE90C0CEF, GameType.DS)]
    public enum ESunCascadeShadowmapOverride : int32
    {
        StandardRenderShadowmap = 1,
        DontRenderShadowmapMakeFullyShadowed = 6,
        DontRenderShadowmapMakeFullyLit = 10,
    }

    [RTTI.Serializable(0xAA724B1A463DDC44, GameType.DS)]
    public enum ESuperLowModelType : int32
    {
        Normal = 0,
        LowTile = 1,
        SuperLowTile = 2,
        Scene = 3,
        AlwaysVisible = 4,
    }

    [RTTI.Serializable(0x4209FFAF89652B45, GameType.DS)]
    public enum ESurfaceDeformationMode : int32
    {
        Mud = 0,
        Snow = 1,
        Disabled = 2,
        Count = 3,
    }

    [RTTI.Serializable(0xE3C8C033A3DF6E6C, GameType.DS)]
    public enum ESwayChange : int32
    {
        MaximalSway = 0,
        SmoothMaximalSway = 1,
        MinimalSway = 2,
        SmoothMinimalSway = 3,
        DontChangeSway = 4,
    }

    [RTTI.Serializable(0x149191FF468BDB93, GameType.DS)]
    public enum ESwitchInputSelectionMethod : int8
    {
        Closest = 0,
        Floor = 1,
        Ceiling = 2,
        WeightRampingInvalid = 3,
    }

    [RTTI.Serializable(0xA9F87D3782A69D10, GameType.DS)]
    public enum ETargetQueryDetail : int8
    {
        DetailedRaycastCheck = 0,
        DetailedShapeCheck = 1,
        CheapShapeCheck = 2,
    }

    [RTTI.Serializable(0x998A4334859D398C, GameType.DS)]
    public enum ETelemetryDamageTracking : int32
    {
        None = 0,
        By_Player = 1,
        By_AI = 2,
        All = 3,
    }

    [RTTI.Serializable(0x75826AECC2CDF66D, GameType.DS)]
    public enum ETerrainBorderStitchingMode : int32
    {
        Skirts = 0,
        IndexBuffer_Stitching = 1,
        Box = 2,
        None = 3,
    }

    [RTTI.Serializable(0x3509F64BCC9D93CF, GameType.DS)]
    public enum ETerrainMaterialLODType : int32
    {
        HighQuality = 0,
        Flattened = 1,
        LowLOD = 2,
    }

    [RTTI.Serializable(0xEE8C101D963FBAFD, GameType.DS)]
    public enum ETerrainTileCullingMode : int32
    {
        ViewCamera = 0,
        None = 1,
    }

    [RTTI.Serializable(0x57512BE59B68CFE7, GameType.DS)]
    public enum ETexAddress : int32
    {
        Wrap = 0,
        Clamp = 1,
        Mirror = 2,
        ClampToBorder = 3,
    }

    [RTTI.Serializable(0x4C5B52F1EADCA82F, GameType.DS)]
    public enum ETexColorSpace : int32
    {
        Linear = 0,
        sRGB = 1,
    }

    [RTTI.Serializable(0xB4B1868FEAA8FF2F, GameType.DS)]
    public enum ETextHAlignment : int32
    {
        _0 = 0,
        left = 1,
        center = 2,
        right = 3,
    }

    [RTTI.Serializable(0x9EF37482788CAD00, GameType.DS)]
    public enum ETextOrientation : int32
    {
        _0 = 0,
        tl_br = 1,
        bl_tr = 2,
        tr_bl = 3,
    }

    [RTTI.Serializable(0x72D17C5609D53967, GameType.DS)]
    public enum ETextOverflow : int32
    {
        _0 = 0,
        visible = 1,
        hidden = 2,
        scroll = 3,
        truncate = 4,
        scaledown = 5,
    }

    [RTTI.Serializable(0xAD05169049168F6C, GameType.DS)]
    public enum ETextTransform : int32
    {
        _0 = 0,
        none = 1,
        capitalize = 4,
        lowercase = 3,
        uppercase = 2,
    }

    [RTTI.Serializable(0xADDDA3B998A4A49C, GameType.DS)]
    public enum ETextWhiteSpace : int32
    {
        _0 = 0,
        normal = 1,
        nowrap = 2,
    }

    [RTTI.Serializable(0xBDB6122A54A9390A, GameType.DS)]
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

    [RTTI.Serializable(0x9ABBCD72A39CBDA8, GameType.DS)]
    public enum ETextureRepeat : int32
    {
        _0 = 0,
        no_repeat = 1,
        repeat_x = 2,
        repeat_y = 3,
        repeat = 4,
    }

    [RTTI.Serializable(0x5355B1EADDB1F3A6, GameType.DS)]
    public enum ETextureSetChannel : int32
    {
        R = 0,
        G = 1,
        B = 2,
        A = 3,
    }

    [RTTI.Serializable(0x7F2504A558A913A9, GameType.DS)]
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
    }

    [RTTI.Serializable(0x360479F071104365, GameType.DS)]
    public enum ETextureSetStorageType : int32
    {
        RGB = 0,
        R = 1,
        G = 2,
        B = 3,
        A = 4,
    }

    [RTTI.Serializable(0x6BD50CD9F2C398B0, GameType.DS)]
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
        Curvature = 14,
        Luminance = 15,
    }

    [RTTI.Serializable(0xCDC39E10FC548E63, GameType.DS)]
    public enum ETextureType : int32
    {
        _2D = 0,
        _3D = 1,
        CubeMap = 2,
        _2DArray = 3,
    }

    [RTTI.Serializable(0x85B079E963333F06, GameType.DS)]
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

    [RTTI.Serializable(0x60A100CE5630C1A4, GameType.DS)]
    public enum EThreatState : int32
    {
        none = -1,
        presence_undetected = 0,
        presence_suspected = 1,
        presence_confirmed = 2,
        threat_identified = 3,
    }

    [RTTI.Serializable(0x32E01FB4B3881232, GameType.DS)]
    public enum ETimeSignatureDenominator : int8
    {
        _1 = 0,
        _2 = 1,
        _4 = 2,
        _8 = 3,
        _16 = 4,
        _32 = 5,
    }

    [RTTI.Serializable(0xAA39BA8739CBDB9D, GameType.DS)]
    public enum ETimerStartType : int32
    {
        Cooked = 0,
        OnEject = 1,
        OnImpact = 2,
    }

    [RTTI.Serializable(0x8D811D5F1289F84A, GameType.DS)]
    public enum EToastMessagePriority : int8
    {
        Info = 0,
        Immediate = 1,
        Exclusive = 2,
    }

    [RTTI.Serializable(0xE27A72B6C583140E, GameType.DS)]
    public enum ETrackingPathUpBlendType : int8
    {
        TerrainToLocalUp = 0,
        TerrainToPathUp = 1,
        PathToLocalUp = 2,
    }

    [RTTI.Serializable(0xC91D2E05B58B7478, GameType.DS)]
    public enum ETrajectorySolveMethod : int32
    {
        Iterative = 0,
        Linear = 1,
    }

    [RTTI.Serializable(0xC188AB30B710354, GameType.DS)]
    public enum ETranslationStatus : int8
    {
        NotApproved = 0,
        GDApproved = 1,
        JapaneseApproved = 2,
        EnglishApproved = 3,
        TranslationApproved = 4,
        QADBApproved = 5,
        QAApproved = 6,
    }

    [RTTI.Serializable(0xAC48E47BE263FF7A, GameType.DS)]
    public enum ETriState : int32
    {
        False = 0,
        True = 1,
        Default = -1,
    }

    [RTTI.Serializable(0x55639246024C0A7D, GameType.DS)]
    public enum ETriggerExposedActionReplication : int32
    {
        ALL_CLIENTS_IF_NETOWNER = 0,
        ALL_CLIENTS = 1,
    }

    [RTTI.Serializable(0x9697E0D54DCF668B, GameType.DS)]
    public enum ETriggerType : int32
    {
        Press = 0,
        Release = 1,
        Continuous = 2,
        Hold = 3,
        Hold_Once = 4,
        Release_NoHold = 5,
        DoubleTap = 6,
        Release_NoDoubleTap = 7,
        None = 8,
    }

    [RTTI.Serializable(0x5A620815FE6E657E, GameType.DS)]
    public enum EUIGlyphShaderAnimationType : int8
    {
        None = 0,
        ShortNeonBlink = 1,
        NormalNeonBlink = 2,
        LongNeonBlink = 3,
        ShortGlitchBlink = 4,
        NormalGlitchBlink = 5,
        LongGlitchBlink = 6,
    }

    [RTTI.Serializable(0xAE7036A4E6A41299, GameType.DS)]
    public enum EUITextScrollType : int8
    {
        None = 0,
        AlwaysScroll = 1,
        FocusedScroll = 2,
        VerticalScroll = 3,
    }

    [RTTI.Serializable(0x1B78E71B85C1F435, GameType.DS)]
    public enum EUpdateFrequency : int8
    {
        _9_99_Hz = 0,
        _14_99_Hz = 1,
        _29_97_Hz = 2,
        _59_94_Hz = 3,
        _120_Hz = 4,
        _144_Hz = 5,
        _240_Hz = 6,
    }

    [RTTI.Serializable(0x738D185BE6064B58, GameType.DS)]
    public enum EUseLocationSelectionSortType : int8
    {
        CenterScreen = 0,
        UserOrientation = 1,
    }

    [RTTI.Serializable(0xD4AD115365F588E5, GameType.DS)]
    public enum EUseLocationType : int32
    {
        General = 0,
        WeaponPickup = 1,
        AutoPickup = 2,
        AmmoPickup = 3,
    }

    [RTTI.Serializable(0xB760E2573ABA5214, GameType.DS)]
    public enum EVAlign : int8
    {
        Default = 0,
        Top = 16,
        Middle = 32,
        Bottom = 48,
    }

    [RTTI.Serializable(0x4B0ADEE9DE2284A7, GameType.DS)]
    public enum EVOLUME_LODLevel : int32
    {
        Level0__4m_ = 0,
        Level1__8m_ = 1,
        Level2__16m_ = 2,
        Level3__32m_ = 3,
        Level4__64m_ = 4,
        Level5__128m_ = 5,
        Level6__256m_ = 6,
    }

    [RTTI.Serializable(0x823A2A3667040242, GameType.DS)]
    public enum EVaultEndInParkourAnnotationDirection : int32
    {
        Parallel = 0,
        Perpendicular = 1,
    }

    [RTTI.Serializable(0x6703FBF3B374181, GameType.DS)]
    public enum EVaultEndInParkourType : int32
    {
        On_Foot_Point = 0,
        On_Foot_Bar = 1,
        Hanging_With_FootSupport = 2,
        Hanging_Without_FootSupport = 3,
    }

    [RTTI.Serializable(0xDCE4FB3364B5D53F, GameType.DS)]
    public enum EVaultObstacleType : int32
    {
        Invalid = -1,
        Vertical = 0,
        Horizontal = 1,
        Parkour = 2,
    }

    [RTTI.Serializable(0xFC42E9F2366039E2, GameType.DS)]
    public enum EVaultType : int32
    {
        Not_Set = -1,
        Step_Over = 0,
        Step_Up = 1,
        Step_Off = 2,
    }

    [RTTI.Serializable(0x6DEC5DC1021EC11, GameType.DS)]
    public enum EVehicleSpecialActionType : int32
    {
        None = 0,
        CatcherTar = 1,
    }

    [RTTI.Serializable(0x3EFA42E97AD6D5B4, GameType.DS)]
    public enum EVehicleType : int32
    {
        None = 0,
        Dummy = 1,
        Truck = 2,
        Motorbike = 3,
    }

    [RTTI.Serializable(0xF3AF7D47725EF5B4, GameType.DS)]
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
        BlendWeights3 = 20,
        BlendIndices3 = 21,
        PivotPoint = 22,
        AltPos = 23,
        AltTangent = 24,
        AltBinormal = 25,
        AltNormal = 26,
        AltColor = 27,
        AltUV0 = 28,
        Invalid = 29,
    }

    [RTTI.Serializable(0x82B0917D65F57B50, GameType.DS)]
    public enum EVertexElementStorageType : int8
    {
        Undefined = 0,
        SignedShortNormalized = 1,
        Float = 2,
        _HalfFloat = 3,
        UnsignedByteNormalized = 4,
        SignedShort = 5,
        X10Y10Z10W2Normalized = 6,
        UnsignedByte = 7,
        UnsignedShort = 8,
        UnsignedShortNormalized = 9,
        UNorm8sRGB = 10,
        X10Y10Z10W2UNorm = 11,
    }

    [RTTI.Serializable(0xA38F534E13232B36, GameType.DS)]
    public enum EVerticalAlignment : int32
    {
        _0 = 0,
        baseline = 1,
        top = 2,
        middle = 3,
        bottom = 4,
        text_bottom = 5,
    }

    [RTTI.Serializable(0xB51DFC9C38E4B54B, GameType.DS)]
    public enum EVictimFactionType : int32
    {
        Friendly = 0,
        Enemy = 1,
        Specific = 2,
    }

    [RTTI.Serializable(0x70727F5D42D174, GameType.DS)]
    public enum EViewLayer : int32
    {
        Background = 0,
        Default = 1,
        FirstPerson = 2,
        Overlay = 3,
    }

    [RTTI.Serializable(0x45565FE06D55D411, GameType.DS)]
    public enum EVoiceLimitMode : int32
    {
        None = 0,
        Stop_Oldest = 1,
        Reject_New = 2,
    }

    [RTTI.Serializable(0x5A541F77CFC4071A, GameType.DS)]
    public enum EVolumetricAnnotationGroup : int32
    {
        None = 0,
        AI_Vision = 1,
        AI_Melee = 2,
        AI_Other = 3,
    }

    [RTTI.Serializable(0xF23DEAAA234548D3, GameType.DS)]
    public enum EWarpedAnimationBoolAnimVarTriggerType : int32
    {
        Trigger_at_start = 0,
        Keep_active = 1,
    }

    [RTTI.Serializable(0x8BD5C28B790EE88D, GameType.DS)]
    public enum EWarpedAnimationDynamicVariableSource : int32
    {
        Rotation_Heading = 0,
        Translation_X = 1,
        Translation_Y = 2,
        Translation_Z = 3,
    }

    [RTTI.Serializable(0x94D914E304690D01, GameType.DS)]
    public enum EWatchTowerAnimeType : int16
    {
        Intro = 0,
        Idle = 1,
        Outro = 2,
    }

    [RTTI.Serializable(0x825353747EFE78B, GameType.DS)]
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

    [RTTI.Serializable(0x15B118418D20EE5E, GameType.DS)]
    public enum EWaveDataEncodingHint : int32
    {
        ATRAC9 = 3,
        MP3 = 4,
        AAC = 6,
        Auto_Select = 7,
    }

    [RTTI.Serializable(0xEACB83E079B3B70E, GameType.DS)]
    public enum EWaveDataEncodingQuality : int32
    {
        Uncompressed__PCM_ = 0,
        Lossy_Lowest = 1,
        Lossy_Low = 2,
        Lossy_Medium = 3,
        Lossy_High = 4,
        Lossy_Highest = 5,
    }

    [RTTI.Serializable(0xC27CC0FE81479C22, GameType.DS)]
    public enum EWeaponStanceRaiseType : int32
    {
        Never = 0,
        Raise_on_start_aim = 1,
        Raise_on_fire = 2,
    }

    [RTTI.Serializable(0x255D9637E064B03C, GameType.DS)]
    public enum EWeaponTriggerType : int32
    {
        Full_Auto = 0,
        Single_Shot_on_Press = 1,
        Single_Shot_on_Release = 2,
    }

    [RTTI.Serializable(0x6B6AD790E79BE1D, GameType.DS)]
    public enum EWidgetLayer : int32
    {
        _0 = 0,
        pre_shader = 1,
        post_shader = 2,
    }

    [RTTI.Serializable(0x12DB317940E2314F, GameType.DS)]
    public enum EWorldDataAccessMode : int32
    {
        Access_By_CPU_Only = 1,
        Access_By_GPU_Only = 2,
        Access_By_CPU_And_GPU = 3,
    }

    [RTTI.Serializable(0xEB620596A84FBC21, GameType.DS)]
    public enum EWorldDataBakeBlendMode : int32
    {
        None = 0,
        Alpha = 1,
        Additive = 2,
        Max = 3,
    }

    [RTTI.Serializable(0x4636C54A79A22431, GameType.DS)]
    public enum EWorldDataDecodingMode : int32
    {
        Default_Decoding = 0,
        NormalMap_Decoding = 1,
    }

    [RTTI.Serializable(0x6B6170EE5E4630A4, GameType.DS)]
    public enum EWorldDataInputLayerApplyMode : int32
    {
        Absolute = 0,
        Additive = 1,
    }

    [RTTI.Serializable(0xD8E0DA88FF61F07A, GameType.DS)]
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

    [RTTI.Serializable(0x878F9984A560129C, GameType.DS)]
    public enum EWorldDataSourceDataMode : int32
    {
        ImageData = 0,
        Generated = 1,
        Baked = 2,
        Painted = 3,
        DefaultValue = 4,
    }

    [RTTI.Serializable(0x25C716D85F08D87D, GameType.DS)]
    public enum EWorldDataTileBorderMode : int32
    {
        Untouched = 0,
        Average = 1,
        AverageStrict = 2,
    }

    [RTTI.Serializable(0x980F3B7A2B254717, GameType.DS)]
    public enum EWwiseGameObjectPositionType : int32
    {
        None = 0,
        Multi_Sources = 1,
        Multi_Directions = 2,
    }

    [RTTI.Serializable(0x23C507B358C5B9D9, GameType.DS)]
    public enum EnvironmentInteractionTargets : int32
    {
        Snow = 1,
        PrecipitationOcclusion = 2,
        Vegetation = 4,
        ForceSystemBit = -2147483648,
    }

    [RTTI.Serializable(0x1F26A2A04479512A, GameType.DS)]
    public enum ForceFieldCategoryMask : int32
    {
        None = 0,
        Wind = 1,
        Particle = 2,
        Vegetation = 4,
        PBD = 8,
        Physics = 16,
        PlantInteraction = 32,
        PresetLocal = 30,
        PresetAll = 63,
    }

    [RTTI.Serializable(0x39343E570AB35561, GameType.DS)]
    public enum ForceFieldProbeSpringMode : int32
    {
        Default = 0,
        Flow = 1,
    }

    [RTTI.Serializable(0x97DBF0180E350D2D, GameType.DS)]
    public enum GameActorLODState : int32
    {
        LOW_LOD = 0,
        MEDIUM_LOD = 1,
        HIGH_LOD = 2,
    }

    [RTTI.Serializable(0xC3FA387A8D01CC1C, GameType.DS)]
    public enum OTGCCommonBaggageState : int8
    {
        IN_BOX = 0,
        LOST = 1,
        DELIVERING = 2,
        ENEMY_DELIVERING = 3,
        ENEMY_BASE = 4,
        DELETED_BAGGAGE = 5,
    }

    [RTTI.Serializable(0x9537DF3D58C3109A, GameType.DS)]
    public enum OTGCCommonEmotionType : int8
    {
        Like = 0,
        ThumbUp = 1,
        Clapping = 2,
        HighFive = 3,
        Hug = 4,
    }

    [RTTI.Serializable(0x2E4BDE05691119E9, GameType.DS)]
    public enum OTGCCommonObjectType : int8
    {
        m = 0,
        z = 1,
        c = 2,
        p = 3,
        a = 4,
        r = 5,
        l = 6,
        s = 7,
        w = 8,
        b = 9,
        t = 10,
        v = 11,
        k = 12,
        n = 13,
        h = 14,
        e = 15,
        u = 16,
        i = 17,
        o = 18,
        x = 19,
    }

    [RTTI.Serializable(0x889FFEC6BD906504, GameType.DS)]
    public enum ParticleRandomSeedMode : int32
    {
        PureRandom = 0,
        CustomAbsolute = 1,
        CustomRelative = 2,
    }

}
