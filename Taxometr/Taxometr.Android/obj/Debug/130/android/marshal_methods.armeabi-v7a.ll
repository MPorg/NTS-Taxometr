; ModuleID = 'obj\Debug\130\android\marshal_methods.armeabi-v7a.ll'
source_filename = "obj\Debug\130\android\marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [238 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 66
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 98
	i32 39852199, ; 2: Plugin.Permissions => 0x26018a7 => 13
	i32 57263871, ; 3: Xamarin.Forms.Core.dll => 0x369c6ff => 93
	i32 101534019, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 82
	i32 120558881, ; 5: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 82
	i32 134690465, ; 6: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 102
	i32 165246403, ; 7: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 43
	i32 166922606, ; 8: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 31
	i32 182336117, ; 9: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 84
	i32 209399409, ; 10: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 41
	i32 230216969, ; 11: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 60
	i32 232815796, ; 12: System.Web.Services => 0xde07cb4 => 115
	i32 261689757, ; 13: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 46
	i32 273568582, ; 14: Plugin.BLE => 0x104e5346 => 8
	i32 278686392, ; 15: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 64
	i32 280482487, ; 16: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 58
	i32 318968648, ; 17: Xamarin.AndroidX.Activity.dll => 0x13031348 => 33
	i32 321597661, ; 18: System.Numerics => 0x132b30dd => 23
	i32 337746723, ; 19: I18N.Other.dll => 0x14219b23 => 118
	i32 342366114, ; 20: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 62
	i32 347068432, ; 21: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 17
	i32 385762202, ; 22: System.Memory.dll => 0x16fe439a => 22
	i32 441335492, ; 23: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 45
	i32 442521989, ; 24: Xamarin.Essentials => 0x1a605985 => 92
	i32 450948140, ; 25: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 57
	i32 465846621, ; 26: mscorlib => 0x1bc4415d => 7
	i32 469710990, ; 27: System.dll => 0x1bff388e => 21
	i32 476646585, ; 28: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 58
	i32 486930444, ; 29: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 70
	i32 514659665, ; 30: Xamarin.Android.Support.Compat => 0x1ead1551 => 31
	i32 526420162, ; 31: System.Transactions.dll => 0x1f6088c2 => 109
	i32 527452488, ; 32: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 102
	i32 605376203, ; 33: System.IO.Compression.FileSystem => 0x24154ecb => 113
	i32 627609679, ; 34: Xamarin.AndroidX.CustomView => 0x2568904f => 51
	i32 639843206, ; 35: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 56
	i32 663517072, ; 36: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 89
	i32 666292255, ; 37: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 38
	i32 690569205, ; 38: System.Xml.Linq.dll => 0x29293ff5 => 28
	i32 691348768, ; 39: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 104
	i32 692692150, ; 40: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 30
	i32 700284507, ; 41: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 99
	i32 720511267, ; 42: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 103
	i32 748832960, ; 43: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 15
	i32 775507847, ; 44: System.IO.Compression => 0x2e394f87 => 112
	i32 809851609, ; 45: System.Drawing.Common.dll => 0x30455ad9 => 111
	i32 843511501, ; 46: Xamarin.AndroidX.Print => 0x3246f6cd => 77
	i32 864477724, ; 47: Plugin.BLE.dll => 0x3386e21c => 8
	i32 907231247, ; 48: Plugin.BluetoothClassic.Android => 0x3613400f => 10
	i32 928116545, ; 49: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 98
	i32 956575887, ; 50: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 103
	i32 957807352, ; 51: Plugin.CurrentActivity => 0x3916faf8 => 11
	i32 967690846, ; 52: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 62
	i32 974778368, ; 53: FormsViewGroup.dll => 0x3a19f000 => 3
	i32 1012816738, ; 54: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 81
	i32 1035644815, ; 55: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 37
	i32 1042160112, ; 56: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 95
	i32 1052210849, ; 57: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 67
	i32 1084122840, ; 58: Xamarin.Kotlin.StdLib => 0x409e66d8 => 101
	i32 1098259244, ; 59: System => 0x41761b2c => 21
	i32 1105113514, ; 60: Honoo.IO.Hashing.Crc.dll => 0x41deb1aa => 4
	i32 1137654822, ; 61: Plugin.Permissions.dll => 0x43cf3c26 => 13
	i32 1175144683, ; 62: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 87
	i32 1178241025, ; 63: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 74
	i32 1204270330, ; 64: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 38
	i32 1264511973, ; 65: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 83
	i32 1267360935, ; 66: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 88
	i32 1275534314, ; 67: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 104
	i32 1282958517, ; 68: Plugin.Geolocator => 0x4c7864b5 => 12
	i32 1292207520, ; 69: SQLitePCLRaw.core.dll => 0x4d0585a0 => 16
	i32 1293217323, ; 70: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 53
	i32 1365406463, ; 71: System.ServiceModel.Internals.dll => 0x516272ff => 106
	i32 1368767823, ; 72: I18N.Other => 0x5195bd4f => 118
	i32 1376866003, ; 73: Xamarin.AndroidX.SavedState => 0x52114ed3 => 81
	i32 1395857551, ; 74: Xamarin.AndroidX.Media.dll => 0x5333188f => 71
	i32 1406073936, ; 75: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 47
	i32 1411638395, ; 76: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 25
	i32 1460219004, ; 77: Xamarin.Forms.Xaml => 0x57092c7c => 96
	i32 1462112819, ; 78: System.IO.Compression.dll => 0x57261233 => 112
	i32 1469204771, ; 79: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 36
	i32 1574652163, ; 80: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 32
	i32 1582372066, ; 81: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 52
	i32 1592978981, ; 82: System.Runtime.Serialization.dll => 0x5ef2ee25 => 2
	i32 1622152042, ; 83: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 69
	i32 1624863272, ; 84: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 91
	i32 1635184631, ; 85: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 56
	i32 1636350590, ; 86: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 50
	i32 1639515021, ; 87: System.Net.Http.dll => 0x61b9038d => 1
	i32 1657153582, ; 88: System.Runtime => 0x62c6282e => 26
	i32 1658241508, ; 89: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 85
	i32 1658251792, ; 90: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 97
	i32 1670060433, ; 91: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 46
	i32 1698840827, ; 92: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 100
	i32 1711441057, ; 93: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 17
	i32 1729485958, ; 94: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 42
	i32 1766324549, ; 95: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 84
	i32 1776026572, ; 96: System.Core.dll => 0x69dc03cc => 20
	i32 1788241197, ; 97: Xamarin.AndroidX.Fragment => 0x6a96652d => 57
	i32 1808609942, ; 98: Xamarin.AndroidX.Loader => 0x6bcd3296 => 69
	i32 1813058853, ; 99: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 101
	i32 1813201214, ; 100: Xamarin.Google.Android.Material => 0x6c13413e => 97
	i32 1818569960, ; 101: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 75
	i32 1867746548, ; 102: Xamarin.Essentials.dll => 0x6f538cf4 => 92
	i32 1873391877, ; 103: Taxometr.dll => 0x6fa9b105 => 29
	i32 1878053835, ; 104: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 96
	i32 1885316902, ; 105: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 39
	i32 1919157823, ; 106: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 72
	i32 1983156543, ; 107: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 100
	i32 2011961780, ; 108: System.Buffers.dll => 0x77ec19b4 => 19
	i32 2019465201, ; 109: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 67
	i32 2055257422, ; 110: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 63
	i32 2067863569, ; 111: I18N.dll => 0x7b411811 => 117
	i32 2079903147, ; 112: System.Runtime.dll => 0x7bf8cdab => 26
	i32 2090596640, ; 113: System.Numerics.Vectors => 0x7c9bf920 => 24
	i32 2097448633, ; 114: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 59
	i32 2103459038, ; 115: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 18
	i32 2126786730, ; 116: Xamarin.Forms.Platform.Android => 0x7ec430aa => 94
	i32 2166116741, ; 117: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 32
	i32 2201107256, ; 118: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 105
	i32 2201231467, ; 119: System.Net.Http => 0x8334206b => 1
	i32 2217644978, ; 120: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 87
	i32 2244775296, ; 121: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 70
	i32 2256548716, ; 122: Xamarin.AndroidX.MultiDex => 0x8680336c => 72
	i32 2261435625, ; 123: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 61
	i32 2279755925, ; 124: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 79
	i32 2315684594, ; 125: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 34
	i32 2403452196, ; 126: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 55
	i32 2409053734, ; 127: Xamarin.AndroidX.Preference.dll => 0x8f973e26 => 76
	i32 2465273461, ; 128: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 15
	i32 2465532216, ; 129: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 45
	i32 2471841756, ; 130: netstandard.dll => 0x93554fdc => 107
	i32 2475788418, ; 131: Java.Interop.dll => 0x93918882 => 5
	i32 2501346920, ; 132: System.Data.DataSetExtensions => 0x95178668 => 110
	i32 2505896520, ; 133: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 66
	i32 2581819634, ; 134: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 88
	i32 2605712449, ; 135: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 105
	i32 2620871830, ; 136: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 50
	i32 2624644809, ; 137: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 54
	i32 2633051222, ; 138: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 64
	i32 2701096212, ; 139: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 85
	i32 2732434636, ; 140: Plugin.BluetoothClassic.Abstractions.dll => 0xa2dda4cc => 9
	i32 2732626843, ; 141: Xamarin.AndroidX.Activity => 0xa2e0939b => 33
	i32 2737747696, ; 142: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 36
	i32 2766581644, ; 143: Xamarin.Forms.Core => 0xa4e6af8c => 93
	i32 2770495804, ; 144: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 99
	i32 2778768386, ; 145: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 90
	i32 2779977773, ; 146: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 80
	i32 2806986428, ; 147: Plugin.CurrentActivity.dll => 0xa74f36bc => 11
	i32 2810250172, ; 148: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 47
	i32 2819470561, ; 149: System.Xml.dll => 0xa80db4e1 => 27
	i32 2821294376, ; 150: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 80
	i32 2853208004, ; 151: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 90
	i32 2855708567, ; 152: Xamarin.AndroidX.Transition => 0xaa36a797 => 86
	i32 2878950252, ; 153: Plugin.BluetoothClassic.Abstractions => 0xab994b6c => 9
	i32 2903344695, ; 154: System.ComponentModel.Composition => 0xad0d8637 => 114
	i32 2905242038, ; 155: mscorlib.dll => 0xad2a79b6 => 7
	i32 2916838712, ; 156: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 91
	i32 2919462931, ; 157: System.Numerics.Vectors.dll => 0xae037813 => 24
	i32 2921128767, ; 158: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 35
	i32 2978675010, ; 159: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 53
	i32 2996846495, ; 160: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 65
	i32 3016983068, ; 161: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 83
	i32 3024354802, ; 162: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 60
	i32 3044182254, ; 163: FormsViewGroup => 0xb57288ee => 3
	i32 3057625584, ; 164: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 73
	i32 3096145461, ; 165: Taxometr.Android => 0xb88b6e35 => 0
	i32 3111772706, ; 166: System.Runtime.Serialization => 0xb979e222 => 2
	i32 3126016514, ; 167: Plugin.Geolocator.dll => 0xba533a02 => 12
	i32 3204380047, ; 168: System.Data.dll => 0xbefef58f => 108
	i32 3211777861, ; 169: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 52
	i32 3247949154, ; 170: Mono.Security => 0xc197c562 => 116
	i32 3258312781, ; 171: Xamarin.AndroidX.CardView => 0xc235e84d => 42
	i32 3267021929, ; 172: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 40
	i32 3286872994, ; 173: SQLite-net.dll => 0xc3e9b3a2 => 14
	i32 3317135071, ; 174: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 51
	i32 3317144872, ; 175: System.Data => 0xc5b79d28 => 108
	i32 3340431453, ; 176: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 39
	i32 3345895724, ; 177: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 78
	i32 3346324047, ; 178: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 74
	i32 3353484488, ; 179: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 59
	i32 3360279109, ; 180: SQLitePCLRaw.core => 0xc849ca45 => 16
	i32 3362522851, ; 181: Xamarin.AndroidX.Core => 0xc86c06e3 => 49
	i32 3366347497, ; 182: Java.Interop => 0xc8a662e9 => 5
	i32 3374999561, ; 183: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 79
	i32 3375054972, ; 184: Plugin.BluetoothClassic.Android.dll => 0xc92b407c => 10
	i32 3395150330, ; 185: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 25
	i32 3404865022, ; 186: System.ServiceModel.Internals => 0xcaf21dfe => 106
	i32 3429136800, ; 187: System.Xml => 0xcc6479a0 => 27
	i32 3430777524, ; 188: netstandard => 0xcc7d82b4 => 107
	i32 3439690031, ; 189: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 30
	i32 3441283291, ; 190: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 54
	i32 3460694193, ; 191: Honoo.IO.Hashing.Crc => 0xce4600b1 => 4
	i32 3476120550, ; 192: Mono.Android => 0xcf3163e6 => 6
	i32 3486566296, ; 193: System.Transactions => 0xcfd0c798 => 109
	i32 3493954962, ; 194: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 44
	i32 3501239056, ; 195: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 40
	i32 3509114376, ; 196: System.Xml.Linq => 0xd128d608 => 28
	i32 3536029504, ; 197: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 94
	i32 3567349600, ; 198: System.ComponentModel.Composition.dll => 0xd4a16f60 => 114
	i32 3579244613, ; 199: I18N => 0xd556f045 => 117
	i32 3618140916, ; 200: Xamarin.AndroidX.Preference => 0xd7a872f4 => 76
	i32 3627220390, ; 201: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 77
	i32 3632359727, ; 202: Xamarin.Forms.Platform => 0xd881692f => 95
	i32 3633644679, ; 203: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 35
	i32 3641597786, ; 204: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 63
	i32 3672681054, ; 205: Mono.Android.dll => 0xdae8aa5e => 6
	i32 3676310014, ; 206: System.Web.Services.dll => 0xdb2009fe => 115
	i32 3682565725, ; 207: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 41
	i32 3684561358, ; 208: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 44
	i32 3689375977, ; 209: System.Drawing.Common => 0xdbe768e9 => 111
	i32 3706696989, ; 210: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 48
	i32 3718780102, ; 211: Xamarin.AndroidX.Annotation => 0xdda814c6 => 34
	i32 3724971120, ; 212: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 73
	i32 3754567612, ; 213: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 18
	i32 3758932259, ; 214: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 61
	i32 3786282454, ; 215: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 43
	i32 3822602673, ; 216: Xamarin.AndroidX.Media => 0xe3d849b1 => 71
	i32 3829621856, ; 217: System.Numerics.dll => 0xe4436460 => 23
	i32 3874194283, ; 218: Taxometr => 0xe6eb836b => 29
	i32 3876362041, ; 219: SQLite-net => 0xe70c9739 => 14
	i32 3885922214, ; 220: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 86
	i32 3888767677, ; 221: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 78
	i32 3896760992, ; 222: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 49
	i32 3920810846, ; 223: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 113
	i32 3921031405, ; 224: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 89
	i32 3931092270, ; 225: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 75
	i32 3945713374, ; 226: System.Data.DataSetExtensions.dll => 0xeb2ecede => 110
	i32 3955647286, ; 227: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 37
	i32 3959773229, ; 228: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 65
	i32 4025784931, ; 229: System.Memory => 0xeff49a63 => 22
	i32 4044736377, ; 230: Taxometr.Android.dll => 0xf115c779 => 0
	i32 4101593132, ; 231: Xamarin.AndroidX.Emoji2 => 0xf479582c => 55
	i32 4105002889, ; 232: Mono.Security.dll => 0xf4ad5f89 => 116
	i32 4151237749, ; 233: System.Core => 0xf76edc75 => 20
	i32 4182413190, ; 234: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 68
	i32 4256097574, ; 235: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 48
	i32 4260525087, ; 236: System.Buffers => 0xfdf2741f => 19
	i32 4292120959 ; 237: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 68
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [238 x i32] [
	i32 66, i32 98, i32 13, i32 93, i32 82, i32 82, i32 102, i32 43, ; 0..7
	i32 31, i32 84, i32 41, i32 60, i32 115, i32 46, i32 8, i32 64, ; 8..15
	i32 58, i32 33, i32 23, i32 118, i32 62, i32 17, i32 22, i32 45, ; 16..23
	i32 92, i32 57, i32 7, i32 21, i32 58, i32 70, i32 31, i32 109, ; 24..31
	i32 102, i32 113, i32 51, i32 56, i32 89, i32 38, i32 28, i32 104, ; 32..39
	i32 30, i32 99, i32 103, i32 15, i32 112, i32 111, i32 77, i32 8, ; 40..47
	i32 10, i32 98, i32 103, i32 11, i32 62, i32 3, i32 81, i32 37, ; 48..55
	i32 95, i32 67, i32 101, i32 21, i32 4, i32 13, i32 87, i32 74, ; 56..63
	i32 38, i32 83, i32 88, i32 104, i32 12, i32 16, i32 53, i32 106, ; 64..71
	i32 118, i32 81, i32 71, i32 47, i32 25, i32 96, i32 112, i32 36, ; 72..79
	i32 32, i32 52, i32 2, i32 69, i32 91, i32 56, i32 50, i32 1, ; 80..87
	i32 26, i32 85, i32 97, i32 46, i32 100, i32 17, i32 42, i32 84, ; 88..95
	i32 20, i32 57, i32 69, i32 101, i32 97, i32 75, i32 92, i32 29, ; 96..103
	i32 96, i32 39, i32 72, i32 100, i32 19, i32 67, i32 63, i32 117, ; 104..111
	i32 26, i32 24, i32 59, i32 18, i32 94, i32 32, i32 105, i32 1, ; 112..119
	i32 87, i32 70, i32 72, i32 61, i32 79, i32 34, i32 55, i32 76, ; 120..127
	i32 15, i32 45, i32 107, i32 5, i32 110, i32 66, i32 88, i32 105, ; 128..135
	i32 50, i32 54, i32 64, i32 85, i32 9, i32 33, i32 36, i32 93, ; 136..143
	i32 99, i32 90, i32 80, i32 11, i32 47, i32 27, i32 80, i32 90, ; 144..151
	i32 86, i32 9, i32 114, i32 7, i32 91, i32 24, i32 35, i32 53, ; 152..159
	i32 65, i32 83, i32 60, i32 3, i32 73, i32 0, i32 2, i32 12, ; 160..167
	i32 108, i32 52, i32 116, i32 42, i32 40, i32 14, i32 51, i32 108, ; 168..175
	i32 39, i32 78, i32 74, i32 59, i32 16, i32 49, i32 5, i32 79, ; 176..183
	i32 10, i32 25, i32 106, i32 27, i32 107, i32 30, i32 54, i32 4, ; 184..191
	i32 6, i32 109, i32 44, i32 40, i32 28, i32 94, i32 114, i32 117, ; 192..199
	i32 76, i32 77, i32 95, i32 35, i32 63, i32 6, i32 115, i32 41, ; 200..207
	i32 44, i32 111, i32 48, i32 34, i32 73, i32 18, i32 61, i32 43, ; 208..215
	i32 71, i32 23, i32 29, i32 14, i32 86, i32 78, i32 49, i32 113, ; 216..223
	i32 89, i32 75, i32 110, i32 37, i32 65, i32 22, i32 0, i32 55, ; 224..231
	i32 116, i32 20, i32 68, i32 48, i32 19, i32 68 ; 232..237
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="all" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ 45b0e144f73b2c8747d8b5ec8cbd3b55beca67f0"}
!llvm.linker.options = !{}
