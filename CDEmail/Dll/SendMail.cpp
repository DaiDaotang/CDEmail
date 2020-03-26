#include "SendMail.h"

static const char base64Char[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

CSendMail::CSendMail()
{
	/*//����ʹ��126��������Ҫ�����ٸ���
	m_STMPAddress = "smtp.126.com";
	//�ܶ������Ĭ�϶˿ڶ���25
	m_STMPPort = 25;
	m_Login = false;*/

}

CSendMail::~CSendMail()
{

}

//base64����
char* base64Encode(char const* origSigned, unsigned origLength)
{
	unsigned char const* orig = (unsigned char const*)origSigned; // in case any input bytes have the MSB set  
	if (orig == NULL) return NULL;

	unsigned const numOrig24BitValues = origLength / 3;
	bool havePadding = origLength > numOrig24BitValues * 3;
	bool havePadding2 = origLength == numOrig24BitValues * 3 + 2;
	unsigned const numResultBytes = 4 * (numOrig24BitValues + havePadding);
	char* result = new char[numResultBytes + 3]; // allow for trailing '/0'  

	// Map each full group of 3 input bytes into 4 output base-64 characters:  
	unsigned i;
	for (i = 0; i < numOrig24BitValues; ++i)
	{
		result[4 * i + 0] = base64Char[(orig[3 * i] >> 2) & 0x3F];
		result[4 * i + 1] = base64Char[(((orig[3 * i] & 0x3) << 4) | (orig[3 * i + 1] >> 4)) & 0x3F];
		result[4 * i + 2] = base64Char[((orig[3 * i + 1] << 2) | (orig[3 * i + 2] >> 6)) & 0x3F];
		result[4 * i + 3] = base64Char[orig[3 * i + 2] & 0x3F];
	}

	// Now, take padding into account.  (Note: i == numOrig24BitValues)  
	if (havePadding)
	{
		result[4 * i + 0] = base64Char[(orig[3 * i] >> 2) & 0x3F];
		if (havePadding2)
		{
			result[4 * i + 1] = base64Char[(((orig[3 * i] & 0x3) << 4) | (orig[3 * i + 1] >> 4)) & 0x3F];
			result[4 * i + 2] = base64Char[(orig[3 * i + 1] << 2) & 0x3F];
		}
		else
		{
			result[4 * i + 1] = base64Char[((orig[3 * i] & 0x3) << 4) & 0x3F];
			result[4 * i + 2] = '=';
		}
		result[4 * i + 3] = '=';
	}

	result[numResultBytes] = '\0';
	return result;
}

// ���õ�ַ��˿�
bool CSendMail::SetSMTP(string address, int port)
{
	m_STMPAddress = address;
	m_STMPPort = port;
	return true;
}

// ��ȡ�������
int CSendMail::GetError(int flags)
{
	Sleep(SLEEPTIME);

	char buff[10240];// ��ʱ����󷵻رȽϳ���������ռ�

	int len = recv(m_SMTPSocket, buff, 10240, 0);

	if (len < 0)
		return len;

	// �����������ս��
	buff[len] = 0;
	string data = buff;
	printf("%s\n", buff);

	// Ascii to int
	int iRe = atoi(data.substr(0, 3).c_str());

	return iRe;
}

// ��¼
bool CSendMail::LoginSMTP(string Email, string Password)
{
	// �˺š����벻Ϊ��
	if (Email.empty() || Password.empty())
		return false;

	m_UserEMail = Email;
	m_PassWord = Password;

	// Socketͨ��֮WSAStartup
	if (WSAStartup(MAKEWORD(2, 0), &m_Wsadata) != 0)
	{
		cout << "StartUp WSA Failed" << endl;
	}

	//����socket��׼��Telnet��smtp
	memset(&m_STMPAddr, 0, sizeof(m_STMPAddr));
	m_SMTPSocket = socket(PF_INET, SOCK_STREAM, 0);

	m_STMPAddr.sin_family = AF_INET;
	m_STMPAddr.sin_port = htons(m_STMPPort);

	// hostent* hptr = gethostbyname("smtp.126.com");
	hostent* hptr = gethostbyname(m_STMPAddress.c_str());

	memcpy(&m_STMPAddr.sin_addr.S_un.S_addr, // Destination
		hptr->h_addr_list[0],			  // Source
		hptr->h_length);				  // Size

	printf("IP of %s is : %d:%d:%d:%d", m_STMPAddress.c_str(), m_STMPAddr.sin_addr.S_un.S_un_b.s_b1, m_STMPAddr.sin_addr.S_un.S_un_b.s_b2, m_STMPAddr.sin_addr.S_un.S_un_b.s_b3, m_STMPAddr.sin_addr.S_un.S_un_b.s_b4);

	//����
	if (connect(m_SMTPSocket, (sockaddr*)&m_STMPAddr, sizeof(m_STMPAddr)) == SOCKET_ERROR)
	{
		cout << "connect failed!" << endl;
		m_Login = false;
		return m_Login;
	}

	//���ý��ճ�ʱ�ȴ�
	int timeout = 200;
	int ret = setsockopt(m_SMTPSocket, SOL_SOCKET, SO_RCVTIMEO, (char*)&timeout, sizeof(int));
	if (ret != 0)
	{
		cout << "Set Failed" << endl;
	}
	if (GetError() != 220)
	{
		cout << "connect failed!" << endl;
	}

	//���ӳɹ�������heloָ�׼����¼
	char bufferHello[] = "helo 126.com\r\n";
	send(m_SMTPSocket, bufferHello, strlen(bufferHello), 0);
	if (GetError() != 250)
	{
		cout << "helo failed!" << endl;
		m_Login = false;
		return m_Login;
	}
	Sleep(SLEEPTIME);

	//׼����¼�������û���������ȫ��
	char bufferLogin[] = "auth login\r\n";
	send(m_SMTPSocket, bufferLogin, strlen(bufferLogin), 0);
	if (GetError() != 334)
	{
		cout << "login failed!" << endl;
		m_Login = false;
		return m_Login;
	}
	Sleep(SLEEPTIME);

	//�����û���
	char user[1024];
	memset(user, NULL, sizeof(char) * 1024);
	string bufferUserName = base64Encode(m_UserEMail.c_str(), m_UserEMail.length());
	bufferUserName += "\r\n";
	strcpy_s(user, bufferUserName.c_str());
	send(m_SMTPSocket, user, strlen(user), 0);
	if (GetError() != 334)
	{
		cout << "UserName Error!" << endl;
		m_Login = false;
		return m_Login;
	}
	Sleep(SLEEPTIME);

	//��������
	string bufferpsw = base64Encode(m_PassWord.c_str(), m_PassWord.length());
	bufferpsw += "\r\n";
	send(m_SMTPSocket, bufferpsw.c_str(), strlen(bufferpsw.c_str()), 0);
	if (GetError() != 235)
	{
		cout << "PassWord Error!" << endl;
		m_Login = false;
		return m_Login;
	}

	m_Login = true;
	return m_Login;
}

// ���ø�����ַ
bool CSendMail::SetEnclPath(vector<string>filename)
{
	m_Filename.clear();
	for (int i = 0; i < filename.size(); i++)
	{
		m_Filename.push_back(filename.at(i));
	}
	return true;
}

// ��д�ʼ�
bool CSendMail::SetTargetEmail(string Email, string title, string body, bool enclosure)
{
	if (!m_Login)
		return false;

	// �󶨷�����
	string bufferMailFrom = "mail from:<" + m_UserEMail + ">\r\n";
	send(m_SMTPSocket, bufferMailFrom.c_str(), strlen(bufferMailFrom.c_str()), 0);
	if (GetError() != 250)
	{
		cout << "SetTargetEmail failed!" << endl;
		return false;
	}
	Sleep(SLEEPTIME);

	// �󶨽�����
	string bufferRcptTo = "rcpt to:<" + Email + ">\r\n";
	send(m_SMTPSocket, bufferRcptTo.c_str(), strlen(bufferRcptTo.c_str()), 0);
	if (GetError() != 250)
	{
		cout << "SetTargetEmail failed!" << endl;
		return false;
	}
	Sleep(SLEEPTIME);

	// ����data����ʾ��ʼ����
	char bufferData[] = "DATA\r\n";
	cout << "data..." << endl;
	send(m_SMTPSocket, bufferData, strlen(bufferData), 0);
	if (GetError(1) != 354)
	{
		send(m_SMTPSocket, bufferData, strlen(bufferData), 0);
	}
	Sleep(SLEEPTIME);

	string message = "from:" + m_UserEMail + "\r\n";
	message += "to:" + Email + "\r\n";
	message += "subject:" + title + "\r\n";
	message += "MIME-Version: 1.0\r\n";
	message += "Content-Type: multipart/mixed;boundary=@boundary@\r\n\r\n";
	send(m_SMTPSocket, message.c_str(), message.length(), 0);
	Sleep(SLEEPTIME);

	//  ����
	string content = body;
	message = "--@boundary@\r\nContent-Type: text/plain;charset=\"gb2312\"\r\n\r\n" + content + "\r\n\r\n";
	send(m_SMTPSocket, message.c_str(), message.length(), 0);
	Sleep(SLEEPTIME);

	if (enclosure)
	{
		SendEnclosure();
	}

	return true;
}

// ����
bool CSendMail::SendEnclosure()
{
	if (!m_Login)
		return false;

	for (int i = 0; i < m_Filename.size(); i++)
	{
		ifstream ifs(m_Filename.at(i), ios::in | ios::binary);
		if (false == ifs.is_open())
		{
			cout << "�޷����ļ���" << endl;
			return 1;
		}
		char  m_cSendBuff[4096];//���ͻ�����

		sprintf_s(m_cSendBuff, "--@boundary@\r\nContent-Type: application/octet-stream;\r\n  name=\"%s\"\r\nContent-Transfer-Encoding: base64\r\nContent-Disposition: attachment;\r\n  filename=\"%s\"\r\n\r\n", m_Filename.at(i).c_str(), m_Filename.at(i).c_str());
		send(m_SMTPSocket, m_cSendBuff, strlen(m_cSendBuff), 0);

		// get length of file:
		ifs.seekg(0, ifs.end);
		int length = ifs.tellg();
		ifs.seekg(0, ifs.beg);
		cout << "length:" << length << endl;
		// allocate memory:
		char* buffer = new char[length];
		// read data as a block:
		ifs.read(buffer, length);
		ifs.close();
		char* pbase = base64Encode(buffer, length);
		delete[]buffer;
		string str(pbase);
		delete[]pbase;
		str += "\r\n";
		int err = send(m_SMTPSocket, str.c_str(), strlen(str.c_str()), 0);
		if (err != strlen(str.c_str()))
		{
			cout << "�������ͳ���!" << endl;
			return 1;
		}
	}
	return true;
}

// �����ʼ�
bool CSendMail::Send()
{
	if (!m_Login)
		return false;

	char bufferPoint[] = "--@boundary@--End--\r\n.\r\n";
	send(m_SMTPSocket, bufferPoint, strlen(bufferPoint), 0);

	int temp;
	if ((temp = GetError(1)) != 250)
	{
		cout << "Send failed!\t" << endl;
		return false;
	}

	return true;
}