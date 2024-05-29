; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [132 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [264 x i32] [
	i32 42639949, ; 0: System.Threading.Thread => 0x28aa24d => 121
	i32 67008169, ; 1: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 33
	i32 72070932, ; 2: Microsoft.Maui.Graphics.dll => 0x44bb714 => 50
	i32 117431740, ; 3: System.Runtime.InteropServices => 0x6ffddbc => 116
	i32 165246403, ; 4: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 62
	i32 182336117, ; 5: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 81
	i32 195452805, ; 6: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 30
	i32 199333315, ; 7: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 31
	i32 205061960, ; 8: System.ComponentModel => 0xc38ff48 => 95
	i32 231409092, ; 9: System.Linq.Parallel => 0xdcb05c4 => 104
	i32 273568582, ; 10: Plugin.BLE => 0x104e5346 => 52
	i32 280992041, ; 11: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 2
	i32 317674968, ; 12: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 30
	i32 318968648, ; 13: Xamarin.AndroidX.Activity.dll => 0x13031348 => 58
	i32 336156722, ; 14: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 15
	i32 342366114, ; 15: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 69
	i32 347068432, ; 16: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 56
	i32 356389973, ; 17: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 14
	i32 379916513, ; 18: System.Threading.Thread.dll => 0x16a510e1 => 121
	i32 385762202, ; 19: System.Memory.dll => 0x16fe439a => 106
	i32 395744057, ; 20: _Microsoft.Android.Resource.Designer => 0x17969339 => 34
	i32 435591531, ; 21: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 26
	i32 442565967, ; 22: System.Collections => 0x1a61054f => 92
	i32 450948140, ; 23: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 68
	i32 456227837, ; 24: System.Web.HttpUtility.dll => 0x1b317bfd => 123
	i32 469710990, ; 25: System.dll => 0x1bff388e => 125
	i32 498788369, ; 26: System.ObjectModel => 0x1dbae811 => 112
	i32 500358224, ; 27: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 13
	i32 503918385, ; 28: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 7
	i32 513247710, ; 29: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 44
	i32 539058512, ; 30: Microsoft.Extensions.Logging => 0x20216150 => 41
	i32 592146354, ; 31: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 21
	i32 597488923, ; 32: CommunityToolkit.Maui => 0x239cf51b => 35
	i32 627609679, ; 33: Xamarin.AndroidX.CustomView => 0x2568904f => 66
	i32 627931235, ; 34: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 19
	i32 672442732, ; 35: System.Collections.Concurrent => 0x2814a96c => 88
	i32 688181140, ; 36: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 1
	i32 706645707, ; 37: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 16
	i32 709557578, ; 38: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 4
	i32 722857257, ; 39: System.Runtime.Loader.dll => 0x2b15ed29 => 117
	i32 748832960, ; 40: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 54
	i32 759454413, ; 41: System.Net.Requests => 0x2d445acd => 110
	i32 775507847, ; 42: System.IO.Compression => 0x2e394f87 => 102
	i32 777317022, ; 43: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 25
	i32 789151979, ; 44: Microsoft.Extensions.Options => 0x2f0980eb => 43
	i32 823281589, ; 45: System.Private.Uri.dll => 0x311247b5 => 113
	i32 830298997, ; 46: System.IO.Compression.Brotli => 0x317d5b75 => 101
	i32 857149644, ; 47: MvvmCross.dll => 0x331710cc => 51
	i32 864477724, ; 48: Plugin.BLE.dll => 0x3386e21c => 52
	i32 904024072, ; 49: System.ComponentModel.Primitives.dll => 0x35e25008 => 93
	i32 926902833, ; 50: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 28
	i32 967690846, ; 51: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 69
	i32 992768348, ; 52: System.Collections.dll => 0x3b2c715c => 92
	i32 1012816738, ; 53: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 80
	i32 1019214401, ; 54: System.Drawing => 0x3cbffa41 => 100
	i32 1028951442, ; 55: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 40
	i32 1029334545, ; 56: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 3
	i32 1035644815, ; 57: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 59
	i32 1036536393, ; 58: System.Drawing.Primitives.dll => 0x3dc84a49 => 99
	i32 1044663988, ; 59: System.Linq.Expressions.dll => 0x3e444eb4 => 103
	i32 1052210849, ; 60: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 71
	i32 1082857460, ; 61: System.ComponentModel.TypeConverter => 0x408b17f4 => 94
	i32 1084122840, ; 62: Xamarin.Kotlin.StdLib => 0x409e66d8 => 85
	i32 1098259244, ; 63: System => 0x41761b2c => 125
	i32 1118262833, ; 64: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 16
	i32 1168523401, ; 65: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 22
	i32 1178241025, ; 66: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 76
	i32 1203215381, ; 67: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 20
	i32 1234928153, ; 68: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 18
	i32 1260983243, ; 69: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 2
	i32 1292207520, ; 70: SQLitePCLRaw.core.dll => 0x4d0585a0 => 55
	i32 1293217323, ; 71: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 67
	i32 1324164729, ; 72: System.Linq => 0x4eed2679 => 105
	i32 1373134921, ; 73: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 32
	i32 1376866003, ; 74: Xamarin.AndroidX.SavedState => 0x52114ed3 => 80
	i32 1406073936, ; 75: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 63
	i32 1430672901, ; 76: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 0
	i32 1461004990, ; 77: es\Microsoft.Maui.Controls.resources => 0x57152abe => 6
	i32 1461234159, ; 78: System.Collections.Immutable.dll => 0x5718a9ef => 89
	i32 1462112819, ; 79: System.IO.Compression.dll => 0x57261233 => 102
	i32 1469204771, ; 80: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 60
	i32 1470490898, ; 81: Microsoft.Extensions.Primitives => 0x57a5e912 => 44
	i32 1479771757, ; 82: System.Collections.Immutable => 0x5833866d => 89
	i32 1480492111, ; 83: System.IO.Compression.Brotli.dll => 0x583e844f => 101
	i32 1493001747, ; 84: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 10
	i32 1514721132, ; 85: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 5
	i32 1543031311, ; 86: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 120
	i32 1551623176, ; 87: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 25
	i32 1622152042, ; 88: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 73
	i32 1624863272, ; 89: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 83
	i32 1634654947, ; 90: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 36
	i32 1636350590, ; 91: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 65
	i32 1639515021, ; 92: System.Net.Http.dll => 0x61b9038d => 107
	i32 1639986890, ; 93: System.Text.RegularExpressions => 0x61c036ca => 120
	i32 1657153582, ; 94: System.Runtime => 0x62c6282e => 118
	i32 1658251792, ; 95: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 84
	i32 1677501392, ; 96: System.Net.Primitives.dll => 0x63fca3d0 => 109
	i32 1679769178, ; 97: System.Security.Cryptography => 0x641f3e5a => 119
	i32 1711441057, ; 98: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 56
	i32 1729485958, ; 99: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 61
	i32 1736233607, ; 100: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 23
	i32 1743415430, ; 101: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 1
	i32 1746316138, ; 102: Mono.Android.Export => 0x6816ab6a => 129
	i32 1763938596, ; 103: System.Diagnostics.TraceSource.dll => 0x69239124 => 98
	i32 1766324549, ; 104: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 81
	i32 1770582343, ; 105: Microsoft.Extensions.Logging.dll => 0x6988f147 => 41
	i32 1780572499, ; 106: Mono.Android.Runtime.dll => 0x6a216153 => 130
	i32 1782862114, ; 107: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 17
	i32 1788241197, ; 108: Xamarin.AndroidX.Fragment => 0x6a96652d => 68
	i32 1793755602, ; 109: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 9
	i32 1808609942, ; 110: Xamarin.AndroidX.Loader => 0x6bcd3296 => 73
	i32 1813058853, ; 111: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 85
	i32 1813201214, ; 112: Xamarin.Google.Android.Material => 0x6c13413e => 84
	i32 1818569960, ; 113: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 77
	i32 1828688058, ; 114: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 42
	i32 1842015223, ; 115: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 29
	i32 1853025655, ; 116: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 26
	i32 1858542181, ; 117: System.Linq.Expressions => 0x6ec71a65 => 103
	i32 1873391877, ; 118: Taxometr.dll => 0x6fa9b105 => 87
	i32 1875935024, ; 119: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 8
	i32 1910275211, ; 120: System.Collections.NonGeneric.dll => 0x71dc7c8b => 90
	i32 1968388702, ; 121: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 37
	i32 2003115576, ; 122: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 5
	i32 2019465201, ; 123: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 71
	i32 2025202353, ; 124: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 0
	i32 2045470958, ; 125: System.Private.Xml => 0x79eb68ee => 114
	i32 2055257422, ; 126: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 70
	i32 2066184531, ; 127: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 4
	i32 2070888862, ; 128: System.Diagnostics.TraceSource => 0x7b6f419e => 98
	i32 2079903147, ; 129: System.Runtime.dll => 0x7bf8cdab => 118
	i32 2090596640, ; 130: System.Numerics.Vectors => 0x7c9bf920 => 111
	i32 2103459038, ; 131: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 57
	i32 2127167465, ; 132: System.Console => 0x7ec9ffe9 => 96
	i32 2142473426, ; 133: System.Collections.Specialized => 0x7fb38cd2 => 91
	i32 2159891885, ; 134: Microsoft.Maui => 0x80bd55ad => 48
	i32 2169148018, ; 135: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 12
	i32 2181898931, ; 136: Microsoft.Extensions.Options.dll => 0x820d22b3 => 43
	i32 2192057212, ; 137: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 42
	i32 2193016926, ; 138: System.ObjectModel.dll => 0x82b6c85e => 112
	i32 2201107256, ; 139: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 86
	i32 2201231467, ; 140: System.Net.Http => 0x8334206b => 107
	i32 2207618523, ; 141: it\Microsoft.Maui.Controls.resources => 0x839595db => 14
	i32 2266799131, ; 142: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 38
	i32 2270573516, ; 143: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 8
	i32 2279755925, ; 144: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 79
	i32 2298471582, ; 145: System.Net.Mail => 0x88ffe49e => 108
	i32 2303942373, ; 146: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 18
	i32 2305521784, ; 147: System.Private.CoreLib.dll => 0x896b7878 => 127
	i32 2340441535, ; 148: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 115
	i32 2353062107, ; 149: System.Net.Primitives => 0x8c40e0db => 109
	i32 2368005991, ; 150: System.Xml.ReaderWriter.dll => 0x8d24e767 => 124
	i32 2371007202, ; 151: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 37
	i32 2395872292, ; 152: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 13
	i32 2401565422, ; 153: System.Web.HttpUtility => 0x8f24faee => 123
	i32 2409053734, ; 154: Xamarin.AndroidX.Preference.dll => 0x8f973e26 => 78
	i32 2427813419, ; 155: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 10
	i32 2435356389, ; 156: System.Console.dll => 0x912896e5 => 96
	i32 2459001652, ; 157: System.Linq.Parallel.dll => 0x92916334 => 104
	i32 2465273461, ; 158: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 54
	i32 2471841756, ; 159: netstandard.dll => 0x93554fdc => 126
	i32 2475788418, ; 160: Java.Interop.dll => 0x93918882 => 128
	i32 2480646305, ; 161: Microsoft.Maui.Controls => 0x93dba8a1 => 46
	i32 2550873716, ; 162: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 11
	i32 2593496499, ; 163: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 20
	i32 2605712449, ; 164: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 86
	i32 2617129537, ; 165: System.Private.Xml.dll => 0x9bfe3a41 => 114
	i32 2620871830, ; 166: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 65
	i32 2626831493, ; 167: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 15
	i32 2663698177, ; 168: System.Runtime.Loader => 0x9ec4cf01 => 117
	i32 2665622720, ; 169: System.Drawing.Primitives => 0x9ee22cc0 => 99
	i32 2732626843, ; 170: Xamarin.AndroidX.Activity => 0xa2e0939b => 58
	i32 2737747696, ; 171: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 60
	i32 2752995522, ; 172: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 21
	i32 2758225723, ; 173: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 47
	i32 2764765095, ; 174: Microsoft.Maui.dll => 0xa4caf7a7 => 48
	i32 2778768386, ; 175: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 82
	i32 2785988530, ; 176: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 27
	i32 2801831435, ; 177: Microsoft.Maui.Graphics => 0xa7008e0b => 50
	i32 2806116107, ; 178: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 6
	i32 2810250172, ; 179: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 63
	i32 2831556043, ; 180: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 19
	i32 2853208004, ; 181: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 82
	i32 2861098320, ; 182: Mono.Android.Export.dll => 0xaa88e550 => 129
	i32 2861189240, ; 183: Microsoft.Maui.Essentials => 0xaa8a4878 => 49
	i32 2868488919, ; 184: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 36
	i32 2909740682, ; 185: System.Private.CoreLib => 0xad6f1e8a => 127
	i32 2916838712, ; 186: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 83
	i32 2919462931, ; 187: System.Numerics.Vectors.dll => 0xae037813 => 111
	i32 2959614098, ; 188: System.ComponentModel.dll => 0xb0682092 => 95
	i32 2978675010, ; 189: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 67
	i32 3038032645, ; 190: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 34
	i32 3057625584, ; 191: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 74
	i32 3059408633, ; 192: Mono.Android.Runtime => 0xb65adef9 => 130
	i32 3059793426, ; 193: System.ComponentModel.Primitives => 0xb660be12 => 93
	i32 3077302341, ; 194: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 12
	i32 3178803400, ; 195: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 75
	i32 3220365878, ; 196: System.Threading => 0xbff2e236 => 122
	i32 3258312781, ; 197: Xamarin.AndroidX.CardView => 0xc235e84d => 61
	i32 3286872994, ; 198: SQLite-net.dll => 0xc3e9b3a2 => 53
	i32 3305363605, ; 199: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 7
	i32 3316684772, ; 200: System.Net.Requests.dll => 0xc5b097e4 => 110
	i32 3317135071, ; 201: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 66
	i32 3346324047, ; 202: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 76
	i32 3357674450, ; 203: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 24
	i32 3360279109, ; 204: SQLitePCLRaw.core => 0xc849ca45 => 55
	i32 3362522851, ; 205: Xamarin.AndroidX.Core => 0xc86c06e3 => 64
	i32 3366347497, ; 206: Java.Interop => 0xc8a662e9 => 128
	i32 3374999561, ; 207: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 79
	i32 3381016424, ; 208: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 3
	i32 3428513518, ; 209: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 39
	i32 3430777524, ; 210: netstandard => 0xcc7d82b4 => 126
	i32 3452344032, ; 211: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 45
	i32 3463511458, ; 212: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 11
	i32 3471940407, ; 213: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 94
	i32 3476120550, ; 214: Mono.Android => 0xcf3163e6 => 131
	i32 3479583265, ; 215: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 24
	i32 3484440000, ; 216: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 23
	i32 3580758918, ; 217: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 31
	i32 3608519521, ; 218: System.Linq.dll => 0xd715a361 => 105
	i32 3618140916, ; 219: Xamarin.AndroidX.Preference => 0xd7a872f4 => 78
	i32 3624195450, ; 220: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 115
	i32 3624345935, ; 221: MvvmCross => 0xd807214f => 51
	i32 3641597786, ; 222: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 70
	i32 3643446276, ; 223: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 28
	i32 3643854240, ; 224: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 75
	i32 3657292374, ; 225: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 38
	i32 3672681054, ; 226: Mono.Android.dll => 0xdae8aa5e => 131
	i32 3697841164, ; 227: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 33
	i32 3724971120, ; 228: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 74
	i32 3748608112, ; 229: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 97
	i32 3754567612, ; 230: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 57
	i32 3786282454, ; 231: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 62
	i32 3792276235, ; 232: System.Collections.NonGeneric => 0xe2098b0b => 90
	i32 3800979733, ; 233: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 45
	i32 3802395368, ; 234: System.Collections.Specialized.dll => 0xe2a3f2e8 => 91
	i32 3817368567, ; 235: CommunityToolkit.Maui.dll => 0xe3886bf7 => 35
	i32 3823082795, ; 236: System.Security.Cryptography.dll => 0xe3df9d2b => 119
	i32 3841636137, ; 237: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 40
	i32 3844307129, ; 238: System.Net.Mail.dll => 0xe52378b9 => 108
	i32 3849253459, ; 239: System.Runtime.InteropServices.dll => 0xe56ef253 => 116
	i32 3874194283, ; 240: Taxometr => 0xe6eb836b => 87
	i32 3876362041, ; 241: SQLite-net => 0xe70c9739 => 53
	i32 3889960447, ; 242: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 32
	i32 3896106733, ; 243: System.Collections.Concurrent.dll => 0xe839deed => 88
	i32 3896760992, ; 244: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 64
	i32 3928044579, ; 245: System.Xml.ReaderWriter => 0xea213423 => 124
	i32 3931092270, ; 246: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 77
	i32 3955647286, ; 247: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 59
	i32 3980434154, ; 248: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 27
	i32 3987592930, ; 249: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 9
	i32 4025784931, ; 250: System.Memory => 0xeff49a63 => 106
	i32 4046471985, ; 251: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 47
	i32 4073602200, ; 252: System.Threading.dll => 0xf2ce3c98 => 122
	i32 4094352644, ; 253: Microsoft.Maui.Essentials.dll => 0xf40add04 => 49
	i32 4099507663, ; 254: System.Drawing.dll => 0xf45985cf => 100
	i32 4100113165, ; 255: System.Private.Uri => 0xf462c30d => 113
	i32 4102112229, ; 256: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 22
	i32 4125707920, ; 257: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 17
	i32 4126470640, ; 258: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 39
	i32 4150914736, ; 259: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 29
	i32 4182413190, ; 260: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 72
	i32 4213026141, ; 261: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 97
	i32 4271975918, ; 262: Microsoft.Maui.Controls.dll => 0xfea12dee => 46
	i32 4292120959 ; 263: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 72
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [264 x i32] [
	i32 121, ; 0
	i32 33, ; 1
	i32 50, ; 2
	i32 116, ; 3
	i32 62, ; 4
	i32 81, ; 5
	i32 30, ; 6
	i32 31, ; 7
	i32 95, ; 8
	i32 104, ; 9
	i32 52, ; 10
	i32 2, ; 11
	i32 30, ; 12
	i32 58, ; 13
	i32 15, ; 14
	i32 69, ; 15
	i32 56, ; 16
	i32 14, ; 17
	i32 121, ; 18
	i32 106, ; 19
	i32 34, ; 20
	i32 26, ; 21
	i32 92, ; 22
	i32 68, ; 23
	i32 123, ; 24
	i32 125, ; 25
	i32 112, ; 26
	i32 13, ; 27
	i32 7, ; 28
	i32 44, ; 29
	i32 41, ; 30
	i32 21, ; 31
	i32 35, ; 32
	i32 66, ; 33
	i32 19, ; 34
	i32 88, ; 35
	i32 1, ; 36
	i32 16, ; 37
	i32 4, ; 38
	i32 117, ; 39
	i32 54, ; 40
	i32 110, ; 41
	i32 102, ; 42
	i32 25, ; 43
	i32 43, ; 44
	i32 113, ; 45
	i32 101, ; 46
	i32 51, ; 47
	i32 52, ; 48
	i32 93, ; 49
	i32 28, ; 50
	i32 69, ; 51
	i32 92, ; 52
	i32 80, ; 53
	i32 100, ; 54
	i32 40, ; 55
	i32 3, ; 56
	i32 59, ; 57
	i32 99, ; 58
	i32 103, ; 59
	i32 71, ; 60
	i32 94, ; 61
	i32 85, ; 62
	i32 125, ; 63
	i32 16, ; 64
	i32 22, ; 65
	i32 76, ; 66
	i32 20, ; 67
	i32 18, ; 68
	i32 2, ; 69
	i32 55, ; 70
	i32 67, ; 71
	i32 105, ; 72
	i32 32, ; 73
	i32 80, ; 74
	i32 63, ; 75
	i32 0, ; 76
	i32 6, ; 77
	i32 89, ; 78
	i32 102, ; 79
	i32 60, ; 80
	i32 44, ; 81
	i32 89, ; 82
	i32 101, ; 83
	i32 10, ; 84
	i32 5, ; 85
	i32 120, ; 86
	i32 25, ; 87
	i32 73, ; 88
	i32 83, ; 89
	i32 36, ; 90
	i32 65, ; 91
	i32 107, ; 92
	i32 120, ; 93
	i32 118, ; 94
	i32 84, ; 95
	i32 109, ; 96
	i32 119, ; 97
	i32 56, ; 98
	i32 61, ; 99
	i32 23, ; 100
	i32 1, ; 101
	i32 129, ; 102
	i32 98, ; 103
	i32 81, ; 104
	i32 41, ; 105
	i32 130, ; 106
	i32 17, ; 107
	i32 68, ; 108
	i32 9, ; 109
	i32 73, ; 110
	i32 85, ; 111
	i32 84, ; 112
	i32 77, ; 113
	i32 42, ; 114
	i32 29, ; 115
	i32 26, ; 116
	i32 103, ; 117
	i32 87, ; 118
	i32 8, ; 119
	i32 90, ; 120
	i32 37, ; 121
	i32 5, ; 122
	i32 71, ; 123
	i32 0, ; 124
	i32 114, ; 125
	i32 70, ; 126
	i32 4, ; 127
	i32 98, ; 128
	i32 118, ; 129
	i32 111, ; 130
	i32 57, ; 131
	i32 96, ; 132
	i32 91, ; 133
	i32 48, ; 134
	i32 12, ; 135
	i32 43, ; 136
	i32 42, ; 137
	i32 112, ; 138
	i32 86, ; 139
	i32 107, ; 140
	i32 14, ; 141
	i32 38, ; 142
	i32 8, ; 143
	i32 79, ; 144
	i32 108, ; 145
	i32 18, ; 146
	i32 127, ; 147
	i32 115, ; 148
	i32 109, ; 149
	i32 124, ; 150
	i32 37, ; 151
	i32 13, ; 152
	i32 123, ; 153
	i32 78, ; 154
	i32 10, ; 155
	i32 96, ; 156
	i32 104, ; 157
	i32 54, ; 158
	i32 126, ; 159
	i32 128, ; 160
	i32 46, ; 161
	i32 11, ; 162
	i32 20, ; 163
	i32 86, ; 164
	i32 114, ; 165
	i32 65, ; 166
	i32 15, ; 167
	i32 117, ; 168
	i32 99, ; 169
	i32 58, ; 170
	i32 60, ; 171
	i32 21, ; 172
	i32 47, ; 173
	i32 48, ; 174
	i32 82, ; 175
	i32 27, ; 176
	i32 50, ; 177
	i32 6, ; 178
	i32 63, ; 179
	i32 19, ; 180
	i32 82, ; 181
	i32 129, ; 182
	i32 49, ; 183
	i32 36, ; 184
	i32 127, ; 185
	i32 83, ; 186
	i32 111, ; 187
	i32 95, ; 188
	i32 67, ; 189
	i32 34, ; 190
	i32 74, ; 191
	i32 130, ; 192
	i32 93, ; 193
	i32 12, ; 194
	i32 75, ; 195
	i32 122, ; 196
	i32 61, ; 197
	i32 53, ; 198
	i32 7, ; 199
	i32 110, ; 200
	i32 66, ; 201
	i32 76, ; 202
	i32 24, ; 203
	i32 55, ; 204
	i32 64, ; 205
	i32 128, ; 206
	i32 79, ; 207
	i32 3, ; 208
	i32 39, ; 209
	i32 126, ; 210
	i32 45, ; 211
	i32 11, ; 212
	i32 94, ; 213
	i32 131, ; 214
	i32 24, ; 215
	i32 23, ; 216
	i32 31, ; 217
	i32 105, ; 218
	i32 78, ; 219
	i32 115, ; 220
	i32 51, ; 221
	i32 70, ; 222
	i32 28, ; 223
	i32 75, ; 224
	i32 38, ; 225
	i32 131, ; 226
	i32 33, ; 227
	i32 74, ; 228
	i32 97, ; 229
	i32 57, ; 230
	i32 62, ; 231
	i32 90, ; 232
	i32 45, ; 233
	i32 91, ; 234
	i32 35, ; 235
	i32 119, ; 236
	i32 40, ; 237
	i32 108, ; 238
	i32 116, ; 239
	i32 87, ; 240
	i32 53, ; 241
	i32 32, ; 242
	i32 88, ; 243
	i32 64, ; 244
	i32 124, ; 245
	i32 77, ; 246
	i32 59, ; 247
	i32 27, ; 248
	i32 9, ; 249
	i32 106, ; 250
	i32 47, ; 251
	i32 122, ; 252
	i32 49, ; 253
	i32 100, ; 254
	i32 113, ; 255
	i32 22, ; 256
	i32 17, ; 257
	i32 39, ; 258
	i32 29, ; 259
	i32 72, ; 260
	i32 97, ; 261
	i32 46, ; 262
	i32 72 ; 263
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.2xx @ 0d97e20b84d8e87c3502469ee395805907905fe3"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
