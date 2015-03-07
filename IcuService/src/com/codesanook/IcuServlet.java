package com.codesanook;

import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

import javax.servlet.http.*;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.ibm.icu.text.BreakIterator;

public class IcuServlet extends HttpServlet {

	@Override
	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws IOException {

		response.setContentType("text/html;charset=utf-8");

		PrintWriter out = response.getWriter();

		String msg = 
				"เพราะยูเลยแล้วจะไปโทษใครเพราะการที่เราจะได้เจอใครซักคนที่ถูกใจ อาจจะพึ่งแค่โชคชะตาและพรหมลิขิตไม่ได้ การเปิดใจและเปิดโอกาสให้ตัวเองได้เข้าไปอยู่ในสถานการณ์ที่เหมาะสม ก็ยังเป็นอีกสิ่งหนึ่งที่อาจจะช่วยให้คุณได้เจอความรักที่กำลังเฝ้ารอ เคยมั้ยคะ? เจอ “คนที่ใช่” ในเวลาที่ผิด มันก็น่าเสียใจและน่าเสียดายnoonswoon premium เป็นผู้ช่วยแก้ปัญหาแบบนี้ให้คุณได้ค่ะ เรารู้ว่า ทุกคนมีความชอบที่แตกต่างและหลากหลาย เราขอเป็นคนเพิ่มโอกาสให้คุณ ได้พบกับคนที่อาจจะถูกใจ ด้วยบริการแนะนำคนพิเศษตามสไตล์ที่คุณชอบ มาคุยกันค่ะว่า คุณชอบใคร ที่มีไลฟสไตล์แบบไหน ทัศนคติหรือมุมมองอย่างไร เผื่อว่า ใครอีกคนหนึ่งก็กำลังตามหาคนแบบคุณอยู่เช่นกัน";
		Locale thaiLocale = new Locale("th");
		String input = msg;
		BreakIterator boundary = BreakIterator.getWordInstance(thaiLocale);
		boundary.setText(input);

		// out.println(printEachForward(boundary, input));
		// http://www.webub.com/%E0%B8%81%E0%B8%B2%E0%B8%A3%E0%B9%83%E0%B8%8A%E0%B9%89%E0%B8%A0%E0%B8%B2%E0%B8%A9%E0%B8%B2%E0%B9%84%E0%B8%97%E0%B8%A2%20%E0%B8%81%E0%B8%B1%E0%B8%9A%20java%20web%20application%20%20servlet%20%20jsp%20%20JSP-1006-49/
		Gson gson = new GsonBuilder().setPrettyPrinting().create();
		String jsonString = gson.toJson(printEachForward(boundary, input));
		out.println(jsonString);
	}

	public List<String> printEachForward(BreakIterator boundary, String source) {

		List<String> list = new ArrayList<String>();
		// StringBuffer strout = new StringBuffer();
		int start = boundary.first();
		for (int end = boundary.next(); end != BreakIterator.DONE; start = end, end = boundary
				.next()) {
			// strout.append(source.substring(start, end)+"-");
			list.add(source.substring(start, end));
		}

		return list;

		// return strout.toString();
	}

}
