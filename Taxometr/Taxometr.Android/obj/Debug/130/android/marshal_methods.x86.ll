; ModuleID = 'obj\Debug\130\android\marshal_methods.x86.ll'
source_filename = "obj\Debug\130\android\marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android"


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
@assembly_image_cache_hashes = local_unnamed_addr constant [212 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 65
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 94
	i32 39852199, ; 2: Plugin.Permissions => 0x26018a7 => 15
	i32 57263871, ; 3: Xamarin.Forms.Core.dll => 0x369c6ff => 89
	i32 101534019, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 79
	i32 120558881, ; 5: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 79
	i32 165246403, ; 6: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 46
	i32 166922606, ; 7: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 34
	i32 182336117, ; 8: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 80
	i32 209399409, ; 9: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 44
	i32 230216969, ; 10: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 60
	i32 232815796, ; 11: System.Web.Services => 0xde07cb4 => 104
	i32 261689757, ; 12: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 49
	i32 273568582, ; 13: Plugin.BLE => 0x104e5346 => 9
	i32 278686392, ; 14: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 64
	i32 280482487, ; 15: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 58
	i32 318968648, ; 16: Xamarin.AndroidX.Activity.dll => 0x13031348 => 36
	i32 321597661, ; 17: System.Numerics => 0x132b30dd => 25
	i32 342366114, ; 18: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 62
	i32 347068432, ; 19: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 19
	i32 385762202, ; 20: System.Memory.dll => 0x16fe439a => 24
	i32 441335492, ; 21: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 48
	i32 442521989, ; 22: Xamarin.Essentials => 0x1a605985 => 88
	i32 450948140, ; 23: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 57
	i32 465846621, ; 24: mscorlib => 0x1bc4415d => 8
	i32 469710990, ; 25: System.dll => 0x1bff388e => 23
	i32 476646585, ; 26: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 58
	i32 486930444, ; 27: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 69
	i32 514659665, ; 28: Xamarin.Android.Support.Compat => 0x1ead1551 => 34
	i32 526420162, ; 29: System.Transactions.dll => 0x1f6088c2 => 99
	i32 605376203, ; 30: System.IO.Compression.FileSystem => 0x24154ecb => 102
	i32 610194910, ; 31: System.Reactive.dll => 0x245ed5de => 27
	i32 627609679, ; 32: Xamarin.AndroidX.CustomView => 0x2568904f => 53
	i32 663517072, ; 33: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 85
	i32 666292255, ; 34: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 41
	i32 690569205, ; 35: System.Xml.Linq.dll => 0x29293ff5 => 31
	i32 692692150, ; 36: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 33
	i32 748832960, ; 37: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 17
	i32 775507847, ; 38: System.IO.Compression => 0x2e394f87 => 101
	i32 809851609, ; 39: System.Drawing.Common.dll => 0x30455ad9 => 96
	i32 843511501, ; 40: Xamarin.AndroidX.Print => 0x3246f6cd => 76
	i32 864477724, ; 41: Plugin.BLE.dll => 0x3386e21c => 9
	i32 907231247, ; 42: Plugin.BluetoothClassic.Android => 0x3613400f => 11
	i32 928116545, ; 43: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 94
	i32 957807352, ; 44: Plugin.CurrentActivity => 0x3916faf8 => 13
	i32 967690846, ; 45: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 62
	i32 974778368, ; 46: FormsViewGroup.dll => 0x3a19f000 => 5
	i32 1012816738, ; 47: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 78
	i32 1035644815, ; 48: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 40
	i32 1042160112, ; 49: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 91
	i32 1052210849, ; 50: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 66
	i32 1098259244, ; 51: System => 0x41761b2c => 23
	i32 1137654822, ; 52: Plugin.Permissions.dll => 0x43cf3c26 => 15
	i32 1175144683, ; 53: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 83
	i32 1178241025, ; 54: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 73
	i32 1204270330, ; 55: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 41
	i32 1267360935, ; 56: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 84
	i32 1282958517, ; 57: Plugin.Geolocator => 0x4c7864b5 => 14
	i32 1292207520, ; 58: SQLitePCLRaw.core.dll => 0x4d0585a0 => 18
	i32 1293217323, ; 59: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 55
	i32 1365406463, ; 60: System.ServiceModel.Internals.dll => 0x516272ff => 95
	i32 1376866003, ; 61: Xamarin.AndroidX.SavedState => 0x52114ed3 => 78
	i32 1395857551, ; 62: Xamarin.AndroidX.Media.dll => 0x5333188f => 70
	i32 1406073936, ; 63: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 50
	i32 1411638395, ; 64: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 28
	i32 1460219004, ; 65: Xamarin.Forms.Xaml => 0x57092c7c => 92
	i32 1462112819, ; 66: System.IO.Compression.dll => 0x57261233 => 101
	i32 1469204771, ; 67: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 39
	i32 1574652163, ; 68: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 35
	i32 1582372066, ; 69: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 54
	i32 1592978981, ; 70: System.Runtime.Serialization.dll => 0x5ef2ee25 => 4
	i32 1622152042, ; 71: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 68
	i32 1624863272, ; 72: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 87
	i32 1636350590, ; 73: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 52
	i32 1639515021, ; 74: System.Net.Http.dll => 0x61b9038d => 3
	i32 1657153582, ; 75: System.Runtime => 0x62c6282e => 29
	i32 1658241508, ; 76: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 81
	i32 1658251792, ; 77: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 93
	i32 1670060433, ; 78: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 49
	i32 1711441057, ; 79: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 19
	i32 1729485958, ; 80: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 45
	i32 1766324549, ; 81: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 80
	i32 1776026572, ; 82: System.Core.dll => 0x69dc03cc => 22
	i32 1788241197, ; 83: Xamarin.AndroidX.Fragment => 0x6a96652d => 57
	i32 1808609942, ; 84: Xamarin.AndroidX.Loader => 0x6bcd3296 => 68
	i32 1813201214, ; 85: Xamarin.Google.Android.Material => 0x6c13413e => 93
	i32 1818569960, ; 86: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 74
	i32 1867746548, ; 87: Xamarin.Essentials.dll => 0x6f538cf4 => 88
	i32 1873391877, ; 88: Taxometr.dll => 0x6fa9b105 => 32
	i32 1878053835, ; 89: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 92
	i32 1885316902, ; 90: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 42
	i32 1904755420, ; 91: System.Runtime.InteropServices.WindowsRuntime.dll => 0x718842dc => 2
	i32 1919157823, ; 92: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 71
	i32 2011961780, ; 93: System.Buffers.dll => 0x77ec19b4 => 21
	i32 2019465201, ; 94: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 66
	i32 2055257422, ; 95: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 63
	i32 2079903147, ; 96: System.Runtime.dll => 0x7bf8cdab => 29
	i32 2090596640, ; 97: System.Numerics.Vectors => 0x7c9bf920 => 26
	i32 2097448633, ; 98: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 59
	i32 2103459038, ; 99: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 20
	i32 2126786730, ; 100: Xamarin.Forms.Platform.Android => 0x7ec430aa => 90
	i32 2166116741, ; 101: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 35
	i32 2201231467, ; 102: System.Net.Http => 0x8334206b => 3
	i32 2217644978, ; 103: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 83
	i32 2244775296, ; 104: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 69
	i32 2256548716, ; 105: Xamarin.AndroidX.MultiDex => 0x8680336c => 71
	i32 2261435625, ; 106: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 61
	i32 2279755925, ; 107: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 77
	i32 2315684594, ; 108: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 37
	i32 2409053734, ; 109: Xamarin.AndroidX.Preference.dll => 0x8f973e26 => 75
	i32 2465273461, ; 110: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 17
	i32 2465532216, ; 111: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 48
	i32 2471841756, ; 112: netstandard.dll => 0x93554fdc => 97
	i32 2475788418, ; 113: Java.Interop.dll => 0x93918882 => 6
	i32 2501346920, ; 114: System.Data.DataSetExtensions => 0x95178668 => 100
	i32 2505896520, ; 115: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 65
	i32 2581819634, ; 116: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 84
	i32 2620871830, ; 117: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 52
	i32 2624644809, ; 118: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 56
	i32 2633051222, ; 119: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 64
	i32 2701096212, ; 120: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 81
	i32 2732434636, ; 121: Plugin.BluetoothClassic.Abstractions.dll => 0xa2dda4cc => 10
	i32 2732626843, ; 122: Xamarin.AndroidX.Activity => 0xa2e0939b => 36
	i32 2737747696, ; 123: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 39
	i32 2750174338, ; 124: Plugin.BluetoothLE.dll => 0xa3ec5482 => 12
	i32 2766581644, ; 125: Xamarin.Forms.Core => 0xa4e6af8c => 89
	i32 2778768386, ; 126: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 86
	i32 2806986428, ; 127: Plugin.CurrentActivity.dll => 0xa74f36bc => 13
	i32 2810250172, ; 128: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 50
	i32 2819470561, ; 129: System.Xml.dll => 0xa80db4e1 => 30
	i32 2853208004, ; 130: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 86
	i32 2855708567, ; 131: Xamarin.AndroidX.Transition => 0xaa36a797 => 82
	i32 2878950252, ; 132: Plugin.BluetoothClassic.Abstractions => 0xab994b6c => 10
	i32 2903344695, ; 133: System.ComponentModel.Composition => 0xad0d8637 => 103
	i32 2905242038, ; 134: mscorlib.dll => 0xad2a79b6 => 8
	i32 2916838712, ; 135: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 87
	i32 2919462931, ; 136: System.Numerics.Vectors.dll => 0xae037813 => 26
	i32 2921128767, ; 137: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 38
	i32 2978675010, ; 138: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 55
	i32 3024354802, ; 139: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 60
	i32 3044182254, ; 140: FormsViewGroup => 0xb57288ee => 5
	i32 3057625584, ; 141: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 72
	i32 3096145461, ; 142: Taxometr.Android => 0xb88b6e35 => 0
	i32 3111772706, ; 143: System.Runtime.Serialization => 0xb979e222 => 4
	i32 3124832203, ; 144: System.Threading.Tasks.Extensions => 0xba4127cb => 1
	i32 3126016514, ; 145: Plugin.Geolocator.dll => 0xba533a02 => 14
	i32 3204380047, ; 146: System.Data.dll => 0xbefef58f => 98
	i32 3211777861, ; 147: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 54
	i32 3247949154, ; 148: Mono.Security => 0xc197c562 => 105
	i32 3258312781, ; 149: Xamarin.AndroidX.CardView => 0xc235e84d => 45
	i32 3265893370, ; 150: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 1
	i32 3267021929, ; 151: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 43
	i32 3286872994, ; 152: SQLite-net.dll => 0xc3e9b3a2 => 16
	i32 3317135071, ; 153: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 53
	i32 3317144872, ; 154: System.Data => 0xc5b79d28 => 98
	i32 3327110942, ; 155: Plugin.BluetoothLE => 0xc64faf1e => 12
	i32 3340431453, ; 156: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 42
	i32 3346324047, ; 157: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 73
	i32 3353484488, ; 158: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 59
	i32 3360279109, ; 159: SQLitePCLRaw.core => 0xc849ca45 => 18
	i32 3362522851, ; 160: Xamarin.AndroidX.Core => 0xc86c06e3 => 51
	i32 3366347497, ; 161: Java.Interop => 0xc8a662e9 => 6
	i32 3374999561, ; 162: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 77
	i32 3375054972, ; 163: Plugin.BluetoothClassic.Android.dll => 0xc92b407c => 11
	i32 3395150330, ; 164: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 28
	i32 3404865022, ; 165: System.ServiceModel.Internals => 0xcaf21dfe => 95
	i32 3429136800, ; 166: System.Xml => 0xcc6479a0 => 30
	i32 3430777524, ; 167: netstandard => 0xcc7d82b4 => 97
	i32 3439690031, ; 168: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 33
	i32 3441283291, ; 169: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 56
	i32 3476120550, ; 170: Mono.Android => 0xcf3163e6 => 7
	i32 3486566296, ; 171: System.Transactions => 0xcfd0c798 => 99
	i32 3493954962, ; 172: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 47
	i32 3501239056, ; 173: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 43
	i32 3509114376, ; 174: System.Xml.Linq => 0xd128d608 => 31
	i32 3536029504, ; 175: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 90
	i32 3567349600, ; 176: System.ComponentModel.Composition.dll => 0xd4a16f60 => 103
	i32 3618140916, ; 177: Xamarin.AndroidX.Preference => 0xd7a872f4 => 75
	i32 3627220390, ; 178: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 76
	i32 3632359727, ; 179: Xamarin.Forms.Platform => 0xd881692f => 91
	i32 3633644679, ; 180: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 38
	i32 3641597786, ; 181: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 63
	i32 3672681054, ; 182: Mono.Android.dll => 0xdae8aa5e => 7
	i32 3676310014, ; 183: System.Web.Services.dll => 0xdb2009fe => 104
	i32 3682565725, ; 184: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 44
	i32 3684561358, ; 185: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 47
	i32 3684933406, ; 186: System.Runtime.InteropServices.WindowsRuntime => 0xdba39f1e => 2
	i32 3689375977, ; 187: System.Drawing.Common => 0xdbe768e9 => 96
	i32 3718780102, ; 188: Xamarin.AndroidX.Annotation => 0xdda814c6 => 37
	i32 3724971120, ; 189: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 72
	i32 3731644420, ; 190: System.Reactive => 0xde6c6004 => 27
	i32 3754567612, ; 191: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 20
	i32 3758932259, ; 192: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 61
	i32 3786282454, ; 193: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 46
	i32 3822602673, ; 194: Xamarin.AndroidX.Media => 0xe3d849b1 => 70
	i32 3829621856, ; 195: System.Numerics.dll => 0xe4436460 => 25
	i32 3874194283, ; 196: Taxometr => 0xe6eb836b => 32
	i32 3876362041, ; 197: SQLite-net => 0xe70c9739 => 16
	i32 3885922214, ; 198: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 82
	i32 3896760992, ; 199: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 51
	i32 3920810846, ; 200: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 102
	i32 3921031405, ; 201: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 85
	i32 3931092270, ; 202: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 74
	i32 3945713374, ; 203: System.Data.DataSetExtensions.dll => 0xeb2ecede => 100
	i32 3955647286, ; 204: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 40
	i32 4025784931, ; 205: System.Memory => 0xeff49a63 => 24
	i32 4044736377, ; 206: Taxometr.Android.dll => 0xf115c779 => 0
	i32 4105002889, ; 207: Mono.Security.dll => 0xf4ad5f89 => 105
	i32 4151237749, ; 208: System.Core => 0xf76edc75 => 22
	i32 4182413190, ; 209: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 67
	i32 4260525087, ; 210: System.Buffers => 0xfdf2741f => 21
	i32 4292120959 ; 211: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 67
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [212 x i32] [
	i32 65, i32 94, i32 15, i32 89, i32 79, i32 79, i32 46, i32 34, ; 0..7
	i32 80, i32 44, i32 60, i32 104, i32 49, i32 9, i32 64, i32 58, ; 8..15
	i32 36, i32 25, i32 62, i32 19, i32 24, i32 48, i32 88, i32 57, ; 16..23
	i32 8, i32 23, i32 58, i32 69, i32 34, i32 99, i32 102, i32 27, ; 24..31
	i32 53, i32 85, i32 41, i32 31, i32 33, i32 17, i32 101, i32 96, ; 32..39
	i32 76, i32 9, i32 11, i32 94, i32 13, i32 62, i32 5, i32 78, ; 40..47
	i32 40, i32 91, i32 66, i32 23, i32 15, i32 83, i32 73, i32 41, ; 48..55
	i32 84, i32 14, i32 18, i32 55, i32 95, i32 78, i32 70, i32 50, ; 56..63
	i32 28, i32 92, i32 101, i32 39, i32 35, i32 54, i32 4, i32 68, ; 64..71
	i32 87, i32 52, i32 3, i32 29, i32 81, i32 93, i32 49, i32 19, ; 72..79
	i32 45, i32 80, i32 22, i32 57, i32 68, i32 93, i32 74, i32 88, ; 80..87
	i32 32, i32 92, i32 42, i32 2, i32 71, i32 21, i32 66, i32 63, ; 88..95
	i32 29, i32 26, i32 59, i32 20, i32 90, i32 35, i32 3, i32 83, ; 96..103
	i32 69, i32 71, i32 61, i32 77, i32 37, i32 75, i32 17, i32 48, ; 104..111
	i32 97, i32 6, i32 100, i32 65, i32 84, i32 52, i32 56, i32 64, ; 112..119
	i32 81, i32 10, i32 36, i32 39, i32 12, i32 89, i32 86, i32 13, ; 120..127
	i32 50, i32 30, i32 86, i32 82, i32 10, i32 103, i32 8, i32 87, ; 128..135
	i32 26, i32 38, i32 55, i32 60, i32 5, i32 72, i32 0, i32 4, ; 136..143
	i32 1, i32 14, i32 98, i32 54, i32 105, i32 45, i32 1, i32 43, ; 144..151
	i32 16, i32 53, i32 98, i32 12, i32 42, i32 73, i32 59, i32 18, ; 152..159
	i32 51, i32 6, i32 77, i32 11, i32 28, i32 95, i32 30, i32 97, ; 160..167
	i32 33, i32 56, i32 7, i32 99, i32 47, i32 43, i32 31, i32 90, ; 168..175
	i32 103, i32 75, i32 76, i32 91, i32 38, i32 63, i32 7, i32 104, ; 176..183
	i32 44, i32 47, i32 2, i32 96, i32 37, i32 72, i32 27, i32 20, ; 184..191
	i32 61, i32 46, i32 70, i32 25, i32 32, i32 16, i32 82, i32 51, ; 192..199
	i32 102, i32 85, i32 74, i32 100, i32 40, i32 24, i32 0, i32 105, ; 200..207
	i32 22, i32 67, i32 21, i32 67 ; 208..211
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="none" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn writeonly
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


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"NumRegisterParameters", i32 0}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ 45b0e144f73b2c8747d8b5ec8cbd3b55beca67f0"}
!llvm.linker.options = !{}
