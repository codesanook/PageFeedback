package com.codesanook;
import java.io.*;
import javax.servlet.http.*;

public class HomeServlet extends HttpServlet {

	@Override
	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws IOException {
		 response.setContentType ("text/html;charset=utf-8");
		PrintWriter out = response.getWriter();
	
		out.println("<html>");
		out.println("<body>");
		out.println("<h1>สวัสดี เพราะยูเลย</h1>");
		out.println("</body>");
		out.println("</html>");
	}

}
