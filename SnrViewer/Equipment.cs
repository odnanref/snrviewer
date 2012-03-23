using System;

namespace SnrViewer
{
	public interface Equipment
	{
		void setType(String Type);
		
		String getType();
		
		int getTotalInterfaces();
		
		void setTotalInterfaces(int total);
		
		void setInterfaceType(System.Collections.ArrayList arrayList);
		
		String getInterfaceType(int i);
		
		System.Collections.ArrayList getInterfaceTypes();
		
		void setName(String Name);
		
		String getName();
		
		void setIp(String Ip);
		
		String getIp();
	}
}

