#include <QJsonArray>
#include <QJsonObject>
#include <QJsonDocument>
#include <QFile>
#include <iostream>
#include <QVector>
#include <QDirIterator>
#include <QTextStream>

using namespace std;

QVector<QString> listDotVersion()
{
	QDirIterator directory("/Volumes/BOOTCAMP/Program Files (x86)/Steam/steamapps/common/Kerbal Space Program/GameData", QDirIterator::Subdirectories);
	QStringList filter("*.version");
	QVector<QString> toRet;

	while(directory.next() != nullptr)
	{
		if(QFileInfo(directory.filePath()).suffix() == "version")
			toRet.append(directory.filePath());
	}
	return toRet;
}


void	writeJSON(QString path, QString JSON)
{
	QFile file;

	file.setFileName(path);
	file.open(QIODevice::WriteOnly | QIODevice::Truncate | QIODevice::Text);

	QTextStream stream(&file);
	stream << JSON;

	file.close();




}

QString	updateJSON(QString path, int major, int minor, int patch)
{
	QFile file;
	QString toRet;
	QJsonObject sd;
	QJsonObject KspVersion;

	file.setFileName(path);
	file.open(QIODevice::ReadOnly | QIODevice::Text);

	sd = QJsonDocument::fromJson(file.readAll()).object();
	file.close();


	KspVersion.insert("MAJOR", major);
	KspVersion.insert("MINOR", minor);
	KspVersion.insert("PATCH", patch);

	sd.remove("KSP_VERSION");
	sd.remove("KSP_VERSION_MIN");
	sd.remove("KSP_VERSION_MAX");

	sd.insert("KSP_VERSION_MIN", KspVersion);
	sd.insert("KSP_VERSION", KspVersion);

	toRet = QJsonDocument(sd).toJson();
	return toRet;
}

int main()
{
	QVector<QString> files = listDotVersion();
	updateJSON(files[0], 1, 6, 1);
	return 0;
}
