#ifndef COMMON_HPP
#define COMMON_HPP


#include <QJsonArray>
#include <QJsonObject>
#include <QJsonDocument>
#include <QFile>
#include <iostream>
#include <QVector>
#include <QDirIterator>
#include <QtNetwork/QNetworkRequest>
#include <QtNetwork/QNetworkReply>
#include <QtNetwork/QNetworkAccessManager>
#include <QThread>

using namespace std;




QString getDownloadLink(QString path);

#endif // COMMON_HPP
