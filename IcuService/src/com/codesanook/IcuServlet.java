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
				"������������Ǩ���������С�÷����Ҩ������ëѡ�����١� �Ҩ�о����⪤�е���о����ԢԵ����� ����Դ�����Դ�͡��������ͧ�����������ʶҹ��ó���������� ���ѧ���ա���˹�觷���Ҩ�Ъ������س���ͤ����ѡ�����ѧ����� �����¤�? �� �������� ����ҷ��Դ �ѹ�����������й�����´��noonswoon premium �繼�������ѭ��Ẻ������س���� ��������� �ء���դ����ͺ���ᵡ��ҧ�����ҡ���� ��Ң��繤������͡�����س �龺�Ѻ������Ҩ�ж١� ���º�ԡ���йӤ�����ɵ��������س�ͺ �Ҥ�¡ѹ������ �س�ͺ�� ������ſ����Ẻ�˹ ��ȹ�����������ͧ���ҧ�� ������� ���ա��˹�觡���ѧ����Ҥ�Ẻ�س�����蹡ѹ";
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
