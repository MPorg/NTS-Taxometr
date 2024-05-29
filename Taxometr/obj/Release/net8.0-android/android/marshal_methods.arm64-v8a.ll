; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [132 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [264 x i64] [
	i64 98382396393917666, ; 0: Microsoft.Extensions.Primitives.dll => 0x15d8644ad360ce2 => 44
	i64 120698629574877762, ; 1: Mono.Android => 0x1accec39cafe242 => 131
	i64 131669012237370309, ; 2: Microsoft.Maui.Essentials.dll => 0x1d3c844de55c3c5 => 49
	i64 196720943101637631, ; 3: System.Linq.Expressions.dll => 0x2bae4a7cd73f3ff => 103
	i64 210515253464952879, ; 4: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 62
	i64 232391251801502327, ; 5: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 80
	i64 545109961164950392, ; 6: fi/Microsoft.Maui.Controls.resources.dll => 0x7909e9f1ec38b78 => 7
	i64 750875890346172408, ; 7: System.Threading.Thread => 0xa6ba5a4da7d1ff8 => 121
	i64 799765834175365804, ; 8: System.ComponentModel.dll => 0xb1956c9f18442ac => 95
	i64 849051935479314978, ; 9: hi/Microsoft.Maui.Controls.resources.dll => 0xbc8703ca21a3a22 => 10
	i64 870603111519317375, ; 10: SQLitePCLRaw.lib.e_sqlite3.android => 0xc1500ead2756d7f => 56
	i64 872800313462103108, ; 11: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 67
	i64 1120440138749646132, ; 12: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 84
	i64 1121665720830085036, ; 13: nb/Microsoft.Maui.Controls.resources.dll => 0xf90f507becf47ac => 18
	i64 1268860745194512059, ; 14: System.Drawing.dll => 0x119be62002c19ebb => 100
	i64 1301485588176585670, ; 15: SQLitePCLRaw.core => 0x120fce3f338e43c6 => 55
	i64 1369545283391376210, ; 16: Xamarin.AndroidX.Navigation.Fragment.dll => 0x13019a2dd85acb52 => 75
	i64 1476839205573959279, ; 17: System.Net.Primitives.dll => 0x147ec96ece9b1e6f => 109
	i64 1486715745332614827, ; 18: Microsoft.Maui.Controls.dll => 0x14a1e017ea87d6ab => 46
	i64 1513467482682125403, ; 19: Mono.Android.Runtime => 0x1500eaa8245f6c5b => 130
	i64 1518315023656898250, ; 20: SQLitePCLRaw.provider.e_sqlite3 => 0x151223783a354eca => 57
	i64 1537168428375924959, ; 21: System.Threading.Thread.dll => 0x15551e8a954ae0df => 121
	i64 1556147632182429976, ; 22: ko/Microsoft.Maui.Controls.resources.dll => 0x15988c06d24c8918 => 16
	i64 1624659445732251991, ; 23: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 60
	i64 1628611045998245443, ; 24: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 72
	i64 1665309068237573118, ; 25: MvvmCross.dll => 0x171c5dc63d898bfe => 51
	i64 1735388228521408345, ; 26: System.Net.Mail.dll => 0x181556663c69b759 => 108
	i64 1743969030606105336, ; 27: System.Memory.dll => 0x1833d297e88f2af8 => 106
	i64 1767386781656293639, ; 28: System.Private.Uri.dll => 0x188704e9f5582107 => 113
	i64 1795316252682057001, ; 29: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 59
	i64 1825687700144851180, ; 30: System.Runtime.InteropServices.RuntimeInformation.dll => 0x1956254a55ef08ec => 115
	i64 1835311033149317475, ; 31: es\Microsoft.Maui.Controls.resources => 0x197855a927386163 => 6
	i64 1836611346387731153, ; 32: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 80
	i64 1875417405349196092, ; 33: System.Drawing.Primitives => 0x1a06d2319b6c713c => 99
	i64 1881198190668717030, ; 34: tr\Microsoft.Maui.Controls.resources => 0x1a1b5bc992ea9be6 => 28
	i64 1920760634179481754, ; 35: Microsoft.Maui.Controls.Xaml => 0x1aa7e99ec2d2709a => 47
	i64 1959996714666907089, ; 36: tr/Microsoft.Maui.Controls.resources.dll => 0x1b334ea0a2a755d1 => 28
	i64 1981742497975770890, ; 37: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 71
	i64 1983698669889758782, ; 38: cs/Microsoft.Maui.Controls.resources.dll => 0x1b87836e2031a63e => 2
	i64 2019660174692588140, ; 39: pl/Microsoft.Maui.Controls.resources.dll => 0x1c07463a6f8e1a6c => 20
	i64 2102659300918482391, ; 40: System.Drawing.Primitives.dll => 0x1d2e257e6aead5d7 => 99
	i64 2262844636196693701, ; 41: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 67
	i64 2287834202362508563, ; 42: System.Collections.Concurrent => 0x1fc00515e8ce7513 => 88
	i64 2302323944321350744, ; 43: ru/Microsoft.Maui.Controls.resources.dll => 0x1ff37f6ddb267c58 => 24
	i64 2329709569556905518, ; 44: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 70
	i64 2470498323731680442, ; 45: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 63
	i64 2497223385847772520, ; 46: System.Runtime => 0x22a7eb7046413568 => 118
	i64 2547086958574651984, ; 47: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 58
	i64 2602673633151553063, ; 48: th\Microsoft.Maui.Controls.resources => 0x241e8de13a460e27 => 27
	i64 2656907746661064104, ; 49: Microsoft.Extensions.DependencyInjection => 0x24df3b84c8b75da8 => 39
	i64 2662981627730767622, ; 50: cs\Microsoft.Maui.Controls.resources => 0x24f4cfae6c48af06 => 2
	i64 2694427813909235223, ; 51: Xamarin.AndroidX.Preference.dll => 0x256487d230fe0617 => 78
	i64 2895129759130297543, ; 52: fi\Microsoft.Maui.Controls.resources => 0x282d912d479fa4c7 => 7
	i64 3017704767998173186, ; 53: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 84
	i64 3289520064315143713, ; 54: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 69
	i64 3311221304742556517, ; 55: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 111
	i64 3344514922410554693, ; 56: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x2e6a1a9a18463545 => 86
	i64 3429672777697402584, ; 57: Microsoft.Maui.Essentials => 0x2f98a5385a7b1ed8 => 49
	i64 3494946837667399002, ; 58: Microsoft.Extensions.Configuration => 0x30808ba1c00a455a => 37
	i64 3522470458906976663, ; 59: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 81
	i64 3551103847008531295, ; 60: System.Private.CoreLib.dll => 0x31480e226177735f => 127
	i64 3567343442040498961, ; 61: pt\Microsoft.Maui.Controls.resources => 0x3181bff5bea4ab11 => 22
	i64 3571415421602489686, ; 62: System.Runtime.dll => 0x319037675df7e556 => 118
	i64 3638003163729360188, ; 63: Microsoft.Extensions.Configuration.Abstractions => 0x327cc89a39d5f53c => 38
	i64 3647754201059316852, ; 64: System.Xml.ReaderWriter => 0x329f6d1e86145474 => 124
	i64 3655542548057982301, ; 65: Microsoft.Extensions.Configuration.dll => 0x32bb18945e52855d => 37
	i64 3716579019761409177, ; 66: netstandard.dll => 0x3393f0ed5c8c5c99 => 126
	i64 3727469159507183293, ; 67: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 79
	i64 3869221888984012293, ; 68: Microsoft.Extensions.Logging.dll => 0x35b23cceda0ed605 => 41
	i64 3890352374528606784, ; 69: Microsoft.Maui.Controls.Xaml.dll => 0x35fd4edf66e00240 => 47
	i64 3933965368022646939, ; 70: System.Net.Requests => 0x369840a8bfadc09b => 110
	i64 3966267475168208030, ; 71: System.Memory => 0x370b03412596249e => 106
	i64 4073500526318903918, ; 72: System.Private.Xml.dll => 0x3887fb25779ae26e => 114
	i64 4073631083018132676, ; 73: Microsoft.Maui.Controls.Compatibility.dll => 0x388871e311491cc4 => 45
	i64 4120493066591692148, ; 74: zh-Hant\Microsoft.Maui.Controls.resources => 0x392eee9cdda86574 => 33
	i64 4154383907710350974, ; 75: System.ComponentModel => 0x39a7562737acb67e => 95
	i64 4187479170553454871, ; 76: System.Linq.Expressions => 0x3a1cea1e912fa117 => 103
	i64 4205801962323029395, ; 77: System.ComponentModel.TypeConverter => 0x3a5e0299f7e7ad93 => 94
	i64 4337444564132831293, ; 78: SQLitePCLRaw.batteries_v2.dll => 0x3c31b2d9ae16203d => 54
	i64 4356591372459378815, ; 79: vi/Microsoft.Maui.Controls.resources.dll => 0x3c75b8c562f9087f => 30
	i64 4477672992252076438, ; 80: System.Web.HttpUtility.dll => 0x3e23e3dcdb8ba196 => 123
	i64 4679594760078841447, ; 81: ar/Microsoft.Maui.Controls.resources.dll => 0x40f142a407475667 => 0
	i64 4794310189461587505, ; 82: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 58
	i64 4795410492532947900, ; 83: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 81
	i64 4809057822547766521, ; 84: System.Drawing => 0x42bd349c3145ecf9 => 100
	i64 4853321196694829351, ; 85: System.Runtime.Loader.dll => 0x435a75ea15de7927 => 117
	i64 5103417709280584325, ; 86: System.Collections.Specialized => 0x46d2fb5e161b6285 => 91
	i64 5182934613077526976, ; 87: System.Collections.Specialized.dll => 0x47ed7b91fa9009c0 => 91
	i64 5290786973231294105, ; 88: System.Runtime.Loader => 0x496ca6b869b72699 => 117
	i64 5423376490970181369, ; 89: System.Runtime.InteropServices.RuntimeInformation => 0x4b43b42f2b7b6ef9 => 115
	i64 5471532531798518949, ; 90: sv\Microsoft.Maui.Controls.resources => 0x4beec9d926d82ca5 => 26
	i64 5522859530602327440, ; 91: uk\Microsoft.Maui.Controls.resources => 0x4ca5237b51eead90 => 29
	i64 5570799893513421663, ; 92: System.IO.Compression.Brotli => 0x4d4f74fcdfa6c35f => 101
	i64 5573260873512690141, ; 93: System.Security.Cryptography.dll => 0x4d58333c6e4ea1dd => 119
	i64 5591791169662171124, ; 94: System.Linq.Parallel => 0x4d9a087135e137f4 => 104
	i64 5692067934154308417, ; 95: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 83
	i64 5814345312393086621, ; 96: Xamarin.AndroidX.Preference => 0x50b0b44182a5c69d => 78
	i64 6068057819846744445, ; 97: ro/Microsoft.Maui.Controls.resources.dll => 0x5436126fec7f197d => 23
	i64 6183170893902868313, ; 98: SQLitePCLRaw.batteries_v2 => 0x55cf092b0c9d6f59 => 54
	i64 6200764641006662125, ; 99: ro\Microsoft.Maui.Controls.resources => 0x560d8a96830131ed => 23
	i64 6357457916754632952, ; 100: _Microsoft.Android.Resource.Designer => 0x583a3a4ac2a7a0f8 => 34
	i64 6401687960814735282, ; 101: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 70
	i64 6478287442656530074, ; 102: hr\Microsoft.Maui.Controls.resources => 0x59e7801b0c6a8e9a => 11
	i64 6548213210057960872, ; 103: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 66
	i64 6560151584539558821, ; 104: Microsoft.Extensions.Options => 0x5b0a571be53243a5 => 43
	i64 6743165466166707109, ; 105: nl\Microsoft.Maui.Controls.resources => 0x5d948943c08c43a5 => 19
	i64 6777482997383978746, ; 106: pt/Microsoft.Maui.Controls.resources.dll => 0x5e0e74e0a2525efa => 22
	i64 6786606130239981554, ; 107: System.Diagnostics.TraceSource => 0x5e2ede51877147f2 => 98
	i64 6894844156784520562, ; 108: System.Numerics.Vectors => 0x5faf683aead1ad72 => 111
	i64 7162901657018122053, ; 109: Taxometr.dll => 0x6367bd1b3b9f3f45 => 87
	i64 7220009545223068405, ; 110: sv/Microsoft.Maui.Controls.resources.dll => 0x6432a06d99f35af5 => 26
	i64 7270811800166795866, ; 111: System.Linq => 0x64e71ccf51a90a5a => 105
	i64 7377312882064240630, ; 112: System.ComponentModel.TypeConverter.dll => 0x66617afac45a2ff6 => 94
	i64 7489048572193775167, ; 113: System.ObjectModel => 0x67ee71ff6b419e3f => 112
	i64 7654504624184590948, ; 114: System.Net.Http => 0x6a3a4366801b8264 => 107
	i64 7694700312542370399, ; 115: System.Net.Mail => 0x6ac9112a7e2cda5f => 108
	i64 7708790323521193081, ; 116: ms/Microsoft.Maui.Controls.resources.dll => 0x6afb1ff4d1730479 => 17
	i64 7714652370974252055, ; 117: System.Private.CoreLib => 0x6b0ff375198b9c17 => 127
	i64 7735352534559001595, ; 118: Xamarin.Kotlin.StdLib.dll => 0x6b597e2582ce8bfb => 85
	i64 7836164640616011524, ; 119: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 60
	i64 8064050204834738623, ; 120: System.Collections.dll => 0x6fe942efa61731bf => 92
	i64 8083354569033831015, ; 121: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 69
	i64 8087206902342787202, ; 122: System.Diagnostics.DiagnosticSource => 0x703b87d46f3aa082 => 97
	i64 8167236081217502503, ; 123: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 128
	i64 8185542183669246576, ; 124: System.Collections => 0x7198e33f4794aa70 => 92
	i64 8246048515196606205, ; 125: Microsoft.Maui.Graphics.dll => 0x726fd96f64ee56fd => 50
	i64 8368701292315763008, ; 126: System.Security.Cryptography => 0x7423997c6fd56140 => 119
	i64 8400357532724379117, ; 127: Xamarin.AndroidX.Navigation.UI.dll => 0x749410ab44503ded => 77
	i64 8518412311883997971, ; 128: System.Collections.Immutable => 0x76377add7c28e313 => 89
	i64 8563666267364444763, ; 129: System.Private.Uri => 0x76d841191140ca5b => 113
	i64 8599632406834268464, ; 130: CommunityToolkit.Maui => 0x7758081c784b4930 => 35
	i64 8614108721271900878, ; 131: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x778b763e14018ace => 21
	i64 8626175481042262068, ; 132: Java.Interop => 0x77b654e585b55834 => 128
	i64 8639588376636138208, ; 133: Xamarin.AndroidX.Navigation.Runtime => 0x77e5fbdaa2fda2e0 => 76
	i64 8677882282824630478, ; 134: pt-BR\Microsoft.Maui.Controls.resources => 0x786e07f5766b00ce => 21
	i64 8725526185868997716, ; 135: System.Diagnostics.DiagnosticSource.dll => 0x79174bd613173454 => 97
	i64 9045785047181495996, ; 136: zh-HK\Microsoft.Maui.Controls.resources => 0x7d891592e3cb0ebc => 31
	i64 9140418186984431467, ; 137: Taxometr => 0x7ed949e9bdba1f6b => 87
	i64 9312692141327339315, ; 138: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 83
	i64 9324707631942237306, ; 139: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 59
	i64 9659729154652888475, ; 140: System.Text.RegularExpressions => 0x860e407c9991dd9b => 120
	i64 9678050649315576968, ; 141: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 63
	i64 9702891218465930390, ; 142: System.Collections.NonGeneric.dll => 0x86a79827b2eb3c96 => 90
	i64 9808709177481450983, ; 143: Mono.Android.dll => 0x881f890734e555e7 => 131
	i64 9956195530459977388, ; 144: Microsoft.Maui => 0x8a2b8315b36616ac => 48
	i64 9991543690424095600, ; 145: es/Microsoft.Maui.Controls.resources.dll => 0x8aa9180c89861370 => 6
	i64 10038780035334861115, ; 146: System.Net.Http.dll => 0x8b50e941206af13b => 107
	i64 10051358222726253779, ; 147: System.Private.Xml => 0x8b7d990c97ccccd3 => 114
	i64 10092835686693276772, ; 148: Microsoft.Maui.Controls => 0x8c10f49539bd0c64 => 46
	i64 10143853363526200146, ; 149: da\Microsoft.Maui.Controls.resources => 0x8cc634e3c2a16b52 => 3
	i64 10229024438826829339, ; 150: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 66
	i64 10406448008575299332, ; 151: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x906b2153fcb3af04 => 86
	i64 10430153318873392755, ; 152: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 64
	i64 10506226065143327199, ; 153: ca\Microsoft.Maui.Controls.resources => 0x91cd9cf11ed169df => 1
	i64 10785150219063592792, ; 154: System.Net.Primitives => 0x95ac8cfb68830758 => 109
	i64 10880838204485145808, ; 155: CommunityToolkit.Maui.dll => 0x970080b2a4d614d0 => 35
	i64 11002576679268595294, ; 156: Microsoft.Extensions.Logging.Abstractions => 0x98b1013215cd365e => 42
	i64 11009005086950030778, ; 157: Microsoft.Maui.dll => 0x98c7d7cc621ffdba => 48
	i64 11103970607964515343, ; 158: hu\Microsoft.Maui.Controls.resources => 0x9a193a6fc41a6c0f => 12
	i64 11162124722117608902, ; 159: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 82
	i64 11220793807500858938, ; 160: ja\Microsoft.Maui.Controls.resources => 0x9bb8448481fdd63a => 15
	i64 11226290749488709958, ; 161: Microsoft.Extensions.Options.dll => 0x9bcbcbf50c874146 => 43
	i64 11340910727871153756, ; 162: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 65
	i64 11446671985764974897, ; 163: Mono.Android.Export => 0x9edabf8623efc131 => 129
	i64 11485890710487134646, ; 164: System.Runtime.InteropServices => 0x9f6614bf0f8b71b6 => 116
	i64 11518296021396496455, ; 165: id\Microsoft.Maui.Controls.resources => 0x9fd9353475222047 => 13
	i64 11529969570048099689, ; 166: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 82
	i64 11530571088791430846, ; 167: Microsoft.Extensions.Logging => 0xa004d1504ccd66be => 41
	i64 11597940890313164233, ; 168: netstandard => 0xa0f429ca8d1805c9 => 126
	i64 11705530742807338875, ; 169: he/Microsoft.Maui.Controls.resources.dll => 0xa272663128721f7b => 9
	i64 11707554492040141440, ; 170: System.Linq.Parallel.dll => 0xa27996c7fe94da80 => 104
	i64 11739066727115742305, ; 171: SQLite-net.dll => 0xa2e98afdf8575c61 => 53
	i64 11806260347154423189, ; 172: SQLite-net => 0xa3d8433bc5eb5d95 => 53
	i64 11953878187842654044, ; 173: Plugin.BLE => 0xa5e4b4e0a2a6cf5c => 52
	i64 12269460666702402136, ; 174: System.Collections.Immutable.dll => 0xaa45e178506c9258 => 89
	i64 12279246230491828964, ; 175: SQLitePCLRaw.provider.e_sqlite3.dll => 0xaa68a5636e0512e4 => 57
	i64 12341818387765915815, ; 176: CommunityToolkit.Maui.Core.dll => 0xab46f26f152bf0a7 => 36
	i64 12451044538927396471, ; 177: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 68
	i64 12466513435562512481, ; 178: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 73
	i64 12475113361194491050, ; 179: _Microsoft.Android.Resource.Designer.dll => 0xad2081818aba1caa => 34
	i64 12517810545449516888, ; 180: System.Diagnostics.TraceSource.dll => 0xadb8325e6f283f58 => 98
	i64 12538491095302438457, ; 181: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 61
	i64 12550732019250633519, ; 182: System.IO.Compression => 0xae2d28465e8e1b2f => 102
	i64 12681088699309157496, ; 183: it/Microsoft.Maui.Controls.resources.dll => 0xaffc46fc178aec78 => 14
	i64 12700543734426720211, ; 184: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 62
	i64 12823819093633476069, ; 185: th/Microsoft.Maui.Controls.resources.dll => 0xb1f75b85abe525e5 => 27
	i64 12843321153144804894, ; 186: Microsoft.Extensions.Primitives => 0xb23ca48abd74d61e => 44
	i64 13221551921002590604, ; 187: ca/Microsoft.Maui.Controls.resources.dll => 0xb77c636bdebe318c => 1
	i64 13222659110913276082, ; 188: ja/Microsoft.Maui.Controls.resources.dll => 0xb78052679c1178b2 => 15
	i64 13343850469010654401, ; 189: Mono.Android.Runtime.dll => 0xb92ee14d854f44c1 => 130
	i64 13381594904270902445, ; 190: he\Microsoft.Maui.Controls.resources => 0xb9b4f9aaad3e94ad => 9
	i64 13384245460423520048, ; 191: Plugin.BLE.dll => 0xb9be64555f2daf30 => 52
	i64 13465488254036897740, ; 192: Xamarin.Kotlin.StdLib => 0xbadf06394d106fcc => 85
	i64 13467053111158216594, ; 193: uk/Microsoft.Maui.Controls.resources.dll => 0xbae49573fde79792 => 29
	i64 13540124433173649601, ; 194: vi\Microsoft.Maui.Controls.resources => 0xbbe82f6eede718c1 => 30
	i64 13545416393490209236, ; 195: id/Microsoft.Maui.Controls.resources.dll => 0xbbfafc7174bc99d4 => 13
	i64 13572454107664307259, ; 196: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 79
	i64 13717397318615465333, ; 197: System.ComponentModel.Primitives.dll => 0xbe5dfc2ef2f87d75 => 93
	i64 13755568601956062840, ; 198: fr/Microsoft.Maui.Controls.resources.dll => 0xbee598c36b1b9678 => 8
	i64 13814445057219246765, ; 199: hr/Microsoft.Maui.Controls.resources.dll => 0xbfb6c49664b43aad => 11
	i64 13881769479078963060, ; 200: System.Console.dll => 0xc0a5f3cade5c6774 => 96
	i64 13959074834287824816, ; 201: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 68
	i64 14100563506285742564, ; 202: da/Microsoft.Maui.Controls.resources.dll => 0xc3af43cd0cff89e4 => 3
	i64 14124974489674258913, ; 203: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 61
	i64 14125464355221830302, ; 204: System.Threading.dll => 0xc407bafdbc707a9e => 122
	i64 14461014870687870182, ; 205: System.Net.Requests.dll => 0xc8afd8683afdece6 => 110
	i64 14464374589798375073, ; 206: ru\Microsoft.Maui.Controls.resources => 0xc8bbc80dcb1e5ea1 => 24
	i64 14522721392235705434, ; 207: el/Microsoft.Maui.Controls.resources.dll => 0xc98b12295c2cf45a => 5
	i64 14556034074661724008, ; 208: CommunityToolkit.Maui.Core => 0xca016bdea6b19f68 => 36
	i64 14669215534098758659, ; 209: Microsoft.Extensions.DependencyInjection.dll => 0xcb9385ceb3993c03 => 39
	i64 14690985099581930927, ; 210: System.Web.HttpUtility => 0xcbe0dd1ca5233daf => 123
	i64 14705122255218365489, ; 211: ko\Microsoft.Maui.Controls.resources => 0xcc1316c7b0fb5431 => 16
	i64 14744092281598614090, ; 212: zh-Hans\Microsoft.Maui.Controls.resources => 0xcc9d89d004439a4a => 32
	i64 14852515768018889994, ; 213: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 65
	i64 14892012299694389861, ; 214: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xceab0e490a083a65 => 33
	i64 14904040806490515477, ; 215: ar\Microsoft.Maui.Controls.resources => 0xced5ca2604cb2815 => 0
	i64 14954917835170835695, ; 216: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xcf8a8a895a82ecef => 40
	i64 14987728460634540364, ; 217: System.IO.Compression.dll => 0xcfff1ba06622494c => 102
	i64 15076659072870671916, ; 218: System.ObjectModel.dll => 0xd13b0d8c1620662c => 112
	i64 15111608613780139878, ; 219: ms\Microsoft.Maui.Controls.resources => 0xd1b737f831192f66 => 17
	i64 15115185479366240210, ; 220: System.IO.Compression.Brotli.dll => 0xd1c3ed1c1bc467d2 => 101
	i64 15133485256822086103, ; 221: System.Linq.dll => 0xd204f0a9127dd9d7 => 105
	i64 15227001540531775957, ; 222: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd3512d3999b8e9d5 => 38
	i64 15370334346939861994, ; 223: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 64
	i64 15391712275433856905, ; 224: Microsoft.Extensions.DependencyInjection.Abstractions => 0xd59a58c406411f89 => 40
	i64 15527772828719725935, ; 225: System.Console => 0xd77dbb1e38cd3d6f => 96
	i64 15536481058354060254, ; 226: de\Microsoft.Maui.Controls.resources => 0xd79cab34eec75bde => 4
	i64 15582737692548360875, ; 227: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 72
	i64 15609085926864131306, ; 228: System.dll => 0xd89e9cf3334914ea => 125
	i64 15661133872274321916, ; 229: System.Xml.ReaderWriter.dll => 0xd9578647d4bfb1fc => 124
	i64 15664356999916475676, ; 230: de/Microsoft.Maui.Controls.resources.dll => 0xd962f9b2b6ecd51c => 4
	i64 15743187114543869802, ; 231: hu/Microsoft.Maui.Controls.resources.dll => 0xda7b09450ae4ef6a => 12
	i64 15783653065526199428, ; 232: el\Microsoft.Maui.Controls.resources => 0xdb0accd674b1c484 => 5
	i64 15928521404965645318, ; 233: Microsoft.Maui.Controls.Compatibility => 0xdd0d79d32c2eec06 => 45
	i64 16154507427712707110, ; 234: System => 0xe03056ea4e39aa26 => 125
	i64 16288847719894691167, ; 235: nb\Microsoft.Maui.Controls.resources => 0xe20d9cb300c12d5f => 18
	i64 16321164108206115771, ; 236: Microsoft.Extensions.Logging.Abstractions.dll => 0xe2806c487e7b0bbb => 42
	i64 16496768397145114574, ; 237: Mono.Android.Export.dll => 0xe4f04b741db987ce => 129
	i64 16649148416072044166, ; 238: Microsoft.Maui.Graphics => 0xe70da84600bb4e86 => 50
	i64 16677317093839702854, ; 239: Xamarin.AndroidX.Navigation.UI => 0xe771bb8960dd8b46 => 77
	i64 16755018182064898362, ; 240: SQLitePCLRaw.core.dll => 0xe885c843c330813a => 55
	i64 16890310621557459193, ; 241: System.Text.RegularExpressions.dll => 0xea66700587f088f9 => 120
	i64 16942731696432749159, ; 242: sk\Microsoft.Maui.Controls.resources => 0xeb20acb622a01a67 => 25
	i64 16998075588627545693, ; 243: Xamarin.AndroidX.Navigation.Fragment => 0xebe54bb02d623e5d => 75
	i64 17008137082415910100, ; 244: System.Collections.NonGeneric => 0xec090a90408c8cd4 => 90
	i64 17031351772568316411, ; 245: Xamarin.AndroidX.Navigation.Common.dll => 0xec5b843380a769fb => 74
	i64 17062143951396181894, ; 246: System.ComponentModel.Primitives => 0xecc8e986518c9786 => 93
	i64 17089008752050867324, ; 247: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xed285aeb25888c7c => 32
	i64 17304527326571357196, ; 248: MvvmCross => 0xf02607eb9253640c => 51
	i64 17342750010158924305, ; 249: hi\Microsoft.Maui.Controls.resources => 0xf0add33f97ecc211 => 10
	i64 17438153253682247751, ; 250: sk/Microsoft.Maui.Controls.resources.dll => 0xf200c3fe308d7847 => 25
	i64 17514990004910432069, ; 251: fr\Microsoft.Maui.Controls.resources => 0xf311be9c6f341f45 => 8
	i64 17623389608345532001, ; 252: pl\Microsoft.Maui.Controls.resources => 0xf492db79dfbef661 => 20
	i64 17702523067201099846, ; 253: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xf5abfef008ae1846 => 31
	i64 17704177640604968747, ; 254: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 73
	i64 17710060891934109755, ; 255: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 71
	i64 17712670374920797664, ; 256: System.Runtime.InteropServices.dll => 0xf5d00bdc38bd3de0 => 116
	i64 18025913125965088385, ; 257: System.Threading => 0xfa28e87b91334681 => 122
	i64 18099568558057551825, ; 258: nl/Microsoft.Maui.Controls.resources.dll => 0xfb2e95b53ad977d1 => 19
	i64 18121036031235206392, ; 259: Xamarin.AndroidX.Navigation.Common => 0xfb7ada42d3d42cf8 => 74
	i64 18245806341561545090, ; 260: System.Collections.Concurrent.dll => 0xfd3620327d587182 => 88
	i64 18305135509493619199, ; 261: Xamarin.AndroidX.Navigation.Runtime.dll => 0xfe08e7c2d8c199ff => 76
	i64 18324163916253801303, ; 262: it\Microsoft.Maui.Controls.resources => 0xfe4c81ff0a56ab57 => 14
	i64 18370042311372477656 ; 263: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0xfeef80274e4094d8 => 56
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [264 x i32] [
	i32 44, ; 0
	i32 131, ; 1
	i32 49, ; 2
	i32 103, ; 3
	i32 62, ; 4
	i32 80, ; 5
	i32 7, ; 6
	i32 121, ; 7
	i32 95, ; 8
	i32 10, ; 9
	i32 56, ; 10
	i32 67, ; 11
	i32 84, ; 12
	i32 18, ; 13
	i32 100, ; 14
	i32 55, ; 15
	i32 75, ; 16
	i32 109, ; 17
	i32 46, ; 18
	i32 130, ; 19
	i32 57, ; 20
	i32 121, ; 21
	i32 16, ; 22
	i32 60, ; 23
	i32 72, ; 24
	i32 51, ; 25
	i32 108, ; 26
	i32 106, ; 27
	i32 113, ; 28
	i32 59, ; 29
	i32 115, ; 30
	i32 6, ; 31
	i32 80, ; 32
	i32 99, ; 33
	i32 28, ; 34
	i32 47, ; 35
	i32 28, ; 36
	i32 71, ; 37
	i32 2, ; 38
	i32 20, ; 39
	i32 99, ; 40
	i32 67, ; 41
	i32 88, ; 42
	i32 24, ; 43
	i32 70, ; 44
	i32 63, ; 45
	i32 118, ; 46
	i32 58, ; 47
	i32 27, ; 48
	i32 39, ; 49
	i32 2, ; 50
	i32 78, ; 51
	i32 7, ; 52
	i32 84, ; 53
	i32 69, ; 54
	i32 111, ; 55
	i32 86, ; 56
	i32 49, ; 57
	i32 37, ; 58
	i32 81, ; 59
	i32 127, ; 60
	i32 22, ; 61
	i32 118, ; 62
	i32 38, ; 63
	i32 124, ; 64
	i32 37, ; 65
	i32 126, ; 66
	i32 79, ; 67
	i32 41, ; 68
	i32 47, ; 69
	i32 110, ; 70
	i32 106, ; 71
	i32 114, ; 72
	i32 45, ; 73
	i32 33, ; 74
	i32 95, ; 75
	i32 103, ; 76
	i32 94, ; 77
	i32 54, ; 78
	i32 30, ; 79
	i32 123, ; 80
	i32 0, ; 81
	i32 58, ; 82
	i32 81, ; 83
	i32 100, ; 84
	i32 117, ; 85
	i32 91, ; 86
	i32 91, ; 87
	i32 117, ; 88
	i32 115, ; 89
	i32 26, ; 90
	i32 29, ; 91
	i32 101, ; 92
	i32 119, ; 93
	i32 104, ; 94
	i32 83, ; 95
	i32 78, ; 96
	i32 23, ; 97
	i32 54, ; 98
	i32 23, ; 99
	i32 34, ; 100
	i32 70, ; 101
	i32 11, ; 102
	i32 66, ; 103
	i32 43, ; 104
	i32 19, ; 105
	i32 22, ; 106
	i32 98, ; 107
	i32 111, ; 108
	i32 87, ; 109
	i32 26, ; 110
	i32 105, ; 111
	i32 94, ; 112
	i32 112, ; 113
	i32 107, ; 114
	i32 108, ; 115
	i32 17, ; 116
	i32 127, ; 117
	i32 85, ; 118
	i32 60, ; 119
	i32 92, ; 120
	i32 69, ; 121
	i32 97, ; 122
	i32 128, ; 123
	i32 92, ; 124
	i32 50, ; 125
	i32 119, ; 126
	i32 77, ; 127
	i32 89, ; 128
	i32 113, ; 129
	i32 35, ; 130
	i32 21, ; 131
	i32 128, ; 132
	i32 76, ; 133
	i32 21, ; 134
	i32 97, ; 135
	i32 31, ; 136
	i32 87, ; 137
	i32 83, ; 138
	i32 59, ; 139
	i32 120, ; 140
	i32 63, ; 141
	i32 90, ; 142
	i32 131, ; 143
	i32 48, ; 144
	i32 6, ; 145
	i32 107, ; 146
	i32 114, ; 147
	i32 46, ; 148
	i32 3, ; 149
	i32 66, ; 150
	i32 86, ; 151
	i32 64, ; 152
	i32 1, ; 153
	i32 109, ; 154
	i32 35, ; 155
	i32 42, ; 156
	i32 48, ; 157
	i32 12, ; 158
	i32 82, ; 159
	i32 15, ; 160
	i32 43, ; 161
	i32 65, ; 162
	i32 129, ; 163
	i32 116, ; 164
	i32 13, ; 165
	i32 82, ; 166
	i32 41, ; 167
	i32 126, ; 168
	i32 9, ; 169
	i32 104, ; 170
	i32 53, ; 171
	i32 53, ; 172
	i32 52, ; 173
	i32 89, ; 174
	i32 57, ; 175
	i32 36, ; 176
	i32 68, ; 177
	i32 73, ; 178
	i32 34, ; 179
	i32 98, ; 180
	i32 61, ; 181
	i32 102, ; 182
	i32 14, ; 183
	i32 62, ; 184
	i32 27, ; 185
	i32 44, ; 186
	i32 1, ; 187
	i32 15, ; 188
	i32 130, ; 189
	i32 9, ; 190
	i32 52, ; 191
	i32 85, ; 192
	i32 29, ; 193
	i32 30, ; 194
	i32 13, ; 195
	i32 79, ; 196
	i32 93, ; 197
	i32 8, ; 198
	i32 11, ; 199
	i32 96, ; 200
	i32 68, ; 201
	i32 3, ; 202
	i32 61, ; 203
	i32 122, ; 204
	i32 110, ; 205
	i32 24, ; 206
	i32 5, ; 207
	i32 36, ; 208
	i32 39, ; 209
	i32 123, ; 210
	i32 16, ; 211
	i32 32, ; 212
	i32 65, ; 213
	i32 33, ; 214
	i32 0, ; 215
	i32 40, ; 216
	i32 102, ; 217
	i32 112, ; 218
	i32 17, ; 219
	i32 101, ; 220
	i32 105, ; 221
	i32 38, ; 222
	i32 64, ; 223
	i32 40, ; 224
	i32 96, ; 225
	i32 4, ; 226
	i32 72, ; 227
	i32 125, ; 228
	i32 124, ; 229
	i32 4, ; 230
	i32 12, ; 231
	i32 5, ; 232
	i32 45, ; 233
	i32 125, ; 234
	i32 18, ; 235
	i32 42, ; 236
	i32 129, ; 237
	i32 50, ; 238
	i32 77, ; 239
	i32 55, ; 240
	i32 120, ; 241
	i32 25, ; 242
	i32 75, ; 243
	i32 90, ; 244
	i32 74, ; 245
	i32 93, ; 246
	i32 32, ; 247
	i32 51, ; 248
	i32 10, ; 249
	i32 25, ; 250
	i32 8, ; 251
	i32 20, ; 252
	i32 31, ; 253
	i32 73, ; 254
	i32 71, ; 255
	i32 116, ; 256
	i32 122, ; 257
	i32 19, ; 258
	i32 74, ; 259
	i32 88, ; 260
	i32 76, ; 261
	i32 14, ; 262
	i32 56 ; 263
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

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
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
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
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.2xx @ 0d97e20b84d8e87c3502469ee395805907905fe3"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
