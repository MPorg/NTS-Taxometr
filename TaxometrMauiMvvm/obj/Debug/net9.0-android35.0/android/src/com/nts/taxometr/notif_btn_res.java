package com.nts.taxometr;


public class notif_btn_res
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("TaxometrMauiMvvm.Services.Background.NotificationButtonReciver, TaxometrMauiMvvm", notif_btn_res.class, __md_methods);
	}

	public notif_btn_res ()
	{
		super ();
		if (getClass () == notif_btn_res.class) {
			mono.android.TypeManager.Activate ("TaxometrMauiMvvm.Services.Background.NotificationButtonReciver, TaxometrMauiMvvm", "", this, new java.lang.Object[] {  });
		}
	}

	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
