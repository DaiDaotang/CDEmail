#pragma once
#ifndef CSENDMAIL_H
#define	CSENDMAIL_H

#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include <winsock2.h>
#pragma comment(lib,"ws2_32.lib")
using namespace std;

#define SLEEPTIME 500

class CSendMail
{
public:
	CSendMail();
	~CSendMail();

	bool SetSMTP(string address, int port);

	//��¼
	bool LoginSMTP(string Email, string Password);

	//���÷���Ŀ�꣬���䣬���⣬���ݣ��Ƿ��и���
	bool SetTargetEmail(string Email, string title, string body, bool enclosure = false);

	//���ø���·��
	bool SetEnclPath(vector<string> filename);

	//����
	bool Send();

protected:
	int  GetError(int flags = 0);
	bool SendEnclosure();

private:
	string m_UserEMail;
	string m_PassWord;
	string m_STMPAddress;
	int    m_STMPPort;
	bool   m_Login;
	vector<string> m_Filename;

	/*---------------Socket---------------*/
	WSADATA     m_Wsadata;
	sockaddr_in m_STMPAddr;
	SOCKET      m_SMTPSocket;
};


#endif