package md52d8426f88d60f9bcdb58df3cbebf9c4a;


public class Contact
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Messaging.Contact, Messaging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Contact.class, __md_methods);
	}


	public Contact () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Contact.class)
			mono.android.TypeManager.Activate ("Messaging.Contact, Messaging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
