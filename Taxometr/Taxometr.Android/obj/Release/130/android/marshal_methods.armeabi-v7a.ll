; ModuleID = 'obj\Release\130\android\marshal_methods.armeabi-v7a.ll'
source_filename = "obj\Release\130\android\marshal_methods.armeabi-v7a.ll"
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
@assembly_image_cache_hashes = local_unnamed_addr constant [136 x i32] [
	i32 34715100, ; 0: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 60
	i32 39852199, ; 1: Plugin.Permissions => 0x26018a7 => 47
	i32 57263871, ; 2: Xamarin.Forms.Core.dll => 0x369c6ff => 56
	i32 134690465, ; 3: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 62
	i32 166922606, ; 4: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 12
	i32 182336117, ; 5: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 29
	i32 273568582, ; 6: Plugin.BLE => 0x104e5346 => 42
	i32 318968648, ; 7: Xamarin.AndroidX.Activity.dll => 0x13031348 => 53
	i32 321597661, ; 8: System.Numerics => 0x132b30dd => 8
	i32 337746723, ; 9: I18N.Other.dll => 0x14219b23 => 39
	i32 342366114, ; 10: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 24
	i32 347068432, ; 11: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 51
	i32 442521989, ; 12: Xamarin.Essentials => 0x1a605985 => 55
	i32 450948140, ; 13: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 22
	i32 465846621, ; 14: mscorlib => 0x1bc4415d => 4
	i32 469710990, ; 15: System.dll => 0x1bff388e => 7
	i32 514659665, ; 16: Xamarin.Android.Support.Compat => 0x1ead1551 => 12
	i32 527452488, ; 17: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 62
	i32 627609679, ; 18: Xamarin.AndroidX.CustomView => 0x2568904f => 20
	i32 663517072, ; 19: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 30
	i32 691348768, ; 20: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 64
	i32 692692150, ; 21: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 11
	i32 700284507, ; 22: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 33
	i32 720511267, ; 23: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 63
	i32 748832960, ; 24: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 49
	i32 809851609, ; 25: System.Drawing.Common.dll => 0x30455ad9 => 36
	i32 864477724, ; 26: Plugin.BLE.dll => 0x3386e21c => 42
	i32 907231247, ; 27: Plugin.BluetoothClassic.Android => 0x3613400f => 44
	i32 928116545, ; 28: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 60
	i32 956575887, ; 29: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 63
	i32 957807352, ; 30: Plugin.CurrentActivity => 0x3916faf8 => 45
	i32 967690846, ; 31: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 24
	i32 974778368, ; 32: FormsViewGroup.dll => 0x3a19f000 => 40
	i32 1012816738, ; 33: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 54
	i32 1035644815, ; 34: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 15
	i32 1042160112, ; 35: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 58
	i32 1052210849, ; 36: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 26
	i32 1084122840, ; 37: Xamarin.Kotlin.StdLib => 0x409e66d8 => 34
	i32 1098259244, ; 38: System => 0x41761b2c => 7
	i32 1105113514, ; 39: Honoo.IO.Hashing.Crc.dll => 0x41deb1aa => 41
	i32 1137654822, ; 40: Plugin.Permissions.dll => 0x43cf3c26 => 47
	i32 1275534314, ; 41: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 64
	i32 1282958517, ; 42: Plugin.Geolocator => 0x4c7864b5 => 46
	i32 1292207520, ; 43: SQLitePCLRaw.core.dll => 0x4d0585a0 => 50
	i32 1293217323, ; 44: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 21
	i32 1365406463, ; 45: System.ServiceModel.Internals.dll => 0x516272ff => 35
	i32 1368767823, ; 46: I18N.Other => 0x5195bd4f => 39
	i32 1376866003, ; 47: Xamarin.AndroidX.SavedState => 0x52114ed3 => 54
	i32 1406073936, ; 48: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 17
	i32 1411638395, ; 49: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 9
	i32 1460219004, ; 50: Xamarin.Forms.Xaml => 0x57092c7c => 59
	i32 1469204771, ; 51: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 14
	i32 1574652163, ; 52: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 13
	i32 1592978981, ; 53: System.Runtime.Serialization.dll => 0x5ef2ee25 => 1
	i32 1622152042, ; 54: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 27
	i32 1636350590, ; 55: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 19
	i32 1639515021, ; 56: System.Net.Http.dll => 0x61b9038d => 0
	i32 1658251792, ; 57: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 32
	i32 1698840827, ; 58: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 61
	i32 1711441057, ; 59: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 51
	i32 1729485958, ; 60: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 16
	i32 1766324549, ; 61: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 29
	i32 1776026572, ; 62: System.Core.dll => 0x69dc03cc => 6
	i32 1788241197, ; 63: Xamarin.AndroidX.Fragment => 0x6a96652d => 22
	i32 1808609942, ; 64: Xamarin.AndroidX.Loader => 0x6bcd3296 => 27
	i32 1813058853, ; 65: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 34
	i32 1813201214, ; 66: Xamarin.Google.Android.Material => 0x6c13413e => 32
	i32 1867746548, ; 67: Xamarin.Essentials.dll => 0x6f538cf4 => 55
	i32 1873391877, ; 68: Taxometr.dll => 0x6fa9b105 => 67
	i32 1878053835, ; 69: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 59
	i32 1983156543, ; 70: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 61
	i32 2011961780, ; 71: System.Buffers.dll => 0x77ec19b4 => 5
	i32 2019465201, ; 72: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 26
	i32 2055257422, ; 73: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 25
	i32 2067863569, ; 74: I18N.dll => 0x7b411811 => 38
	i32 2097448633, ; 75: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 23
	i32 2103459038, ; 76: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 52
	i32 2126786730, ; 77: Xamarin.Forms.Platform.Android => 0x7ec430aa => 57
	i32 2166116741, ; 78: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 13
	i32 2201107256, ; 79: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 65
	i32 2201231467, ; 80: System.Net.Http => 0x8334206b => 0
	i32 2279755925, ; 81: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 28
	i32 2465273461, ; 82: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 49
	i32 2475788418, ; 83: Java.Interop.dll => 0x93918882 => 2
	i32 2605712449, ; 84: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 65
	i32 2620871830, ; 85: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 19
	i32 2732434636, ; 86: Plugin.BluetoothClassic.Abstractions.dll => 0xa2dda4cc => 43
	i32 2732626843, ; 87: Xamarin.AndroidX.Activity => 0xa2e0939b => 53
	i32 2737747696, ; 88: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 14
	i32 2766581644, ; 89: Xamarin.Forms.Core => 0xa4e6af8c => 56
	i32 2770495804, ; 90: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 33
	i32 2778768386, ; 91: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 31
	i32 2806986428, ; 92: Plugin.CurrentActivity.dll => 0xa74f36bc => 45
	i32 2810250172, ; 93: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 17
	i32 2819470561, ; 94: System.Xml.dll => 0xa80db4e1 => 10
	i32 2853208004, ; 95: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 31
	i32 2878950252, ; 96: Plugin.BluetoothClassic.Abstractions => 0xab994b6c => 43
	i32 2905242038, ; 97: mscorlib.dll => 0xad2a79b6 => 4
	i32 2978675010, ; 98: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 21
	i32 3044182254, ; 99: FormsViewGroup => 0xb57288ee => 40
	i32 3096145461, ; 100: Taxometr.Android => 0xb88b6e35 => 66
	i32 3111772706, ; 101: System.Runtime.Serialization => 0xb979e222 => 1
	i32 3126016514, ; 102: Plugin.Geolocator.dll => 0xba533a02 => 46
	i32 3247949154, ; 103: Mono.Security => 0xc197c562 => 37
	i32 3258312781, ; 104: Xamarin.AndroidX.CardView => 0xc235e84d => 16
	i32 3286872994, ; 105: SQLite-net.dll => 0xc3e9b3a2 => 48
	i32 3317135071, ; 106: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 20
	i32 3353484488, ; 107: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 23
	i32 3360279109, ; 108: SQLitePCLRaw.core => 0xc849ca45 => 50
	i32 3362522851, ; 109: Xamarin.AndroidX.Core => 0xc86c06e3 => 18
	i32 3366347497, ; 110: Java.Interop => 0xc8a662e9 => 2
	i32 3374999561, ; 111: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 28
	i32 3375054972, ; 112: Plugin.BluetoothClassic.Android.dll => 0xc92b407c => 44
	i32 3395150330, ; 113: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 9
	i32 3404865022, ; 114: System.ServiceModel.Internals => 0xcaf21dfe => 35
	i32 3429136800, ; 115: System.Xml => 0xcc6479a0 => 10
	i32 3439690031, ; 116: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 11
	i32 3460694193, ; 117: Honoo.IO.Hashing.Crc => 0xce4600b1 => 41
	i32 3476120550, ; 118: Mono.Android => 0xcf3163e6 => 3
	i32 3536029504, ; 119: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 57
	i32 3579244613, ; 120: I18N => 0xd556f045 => 38
	i32 3632359727, ; 121: Xamarin.Forms.Platform => 0xd881692f => 58
	i32 3641597786, ; 122: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 25
	i32 3672681054, ; 123: Mono.Android.dll => 0xdae8aa5e => 3
	i32 3689375977, ; 124: System.Drawing.Common => 0xdbe768e9 => 36
	i32 3754567612, ; 125: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 52
	i32 3829621856, ; 126: System.Numerics.dll => 0xe4436460 => 8
	i32 3874194283, ; 127: Taxometr => 0xe6eb836b => 67
	i32 3876362041, ; 128: SQLite-net => 0xe70c9739 => 48
	i32 3896760992, ; 129: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 18
	i32 3921031405, ; 130: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 30
	i32 3955647286, ; 131: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 15
	i32 4044736377, ; 132: Taxometr.Android.dll => 0xf115c779 => 66
	i32 4105002889, ; 133: Mono.Security.dll => 0xf4ad5f89 => 37
	i32 4151237749, ; 134: System.Core => 0xf76edc75 => 6
	i32 4260525087 ; 135: System.Buffers => 0xfdf2741f => 5
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [136 x i32] [
	i32 60, i32 47, i32 56, i32 62, i32 12, i32 29, i32 42, i32 53, ; 0..7
	i32 8, i32 39, i32 24, i32 51, i32 55, i32 22, i32 4, i32 7, ; 8..15
	i32 12, i32 62, i32 20, i32 30, i32 64, i32 11, i32 33, i32 63, ; 16..23
	i32 49, i32 36, i32 42, i32 44, i32 60, i32 63, i32 45, i32 24, ; 24..31
	i32 40, i32 54, i32 15, i32 58, i32 26, i32 34, i32 7, i32 41, ; 32..39
	i32 47, i32 64, i32 46, i32 50, i32 21, i32 35, i32 39, i32 54, ; 40..47
	i32 17, i32 9, i32 59, i32 14, i32 13, i32 1, i32 27, i32 19, ; 48..55
	i32 0, i32 32, i32 61, i32 51, i32 16, i32 29, i32 6, i32 22, ; 56..63
	i32 27, i32 34, i32 32, i32 55, i32 67, i32 59, i32 61, i32 5, ; 64..71
	i32 26, i32 25, i32 38, i32 23, i32 52, i32 57, i32 13, i32 65, ; 72..79
	i32 0, i32 28, i32 49, i32 2, i32 65, i32 19, i32 43, i32 53, ; 80..87
	i32 14, i32 56, i32 33, i32 31, i32 45, i32 17, i32 10, i32 31, ; 88..95
	i32 43, i32 4, i32 21, i32 40, i32 66, i32 1, i32 46, i32 37, ; 96..103
	i32 16, i32 48, i32 20, i32 23, i32 50, i32 18, i32 2, i32 28, ; 104..111
	i32 44, i32 9, i32 35, i32 10, i32 11, i32 41, i32 3, i32 57, ; 112..119
	i32 38, i32 58, i32 25, i32 3, i32 36, i32 52, i32 8, i32 67, ; 120..127
	i32 48, i32 18, i32 30, i32 15, i32 66, i32 37, i32 6, i32 5 ; 136..135
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
