#pragma once
#include "stdafx.h"
#include "INotificationListener.h"
#include "../Utilities/SimpleLock.h"

typedef void(__stdcall* OpExecSyncCallback)(void*, void*, void*, void*, void*, void*, void*, void*);

class NotificationManager
{
private:
	SimpleLock _lock;
	vector<weak_ptr<INotificationListener>> _listenersToAdd;
	vector<weak_ptr<INotificationListener>> _listeners;
	
	void CleanupNotificationListeners();

public:
	void RegisterNotificationListener(shared_ptr<INotificationListener> notificationListener);
	void SendNotification(ConsoleNotificationType type, void* parameter = nullptr);
	void RegisterOpExecSync(OpExecSyncCallback callback);
	void OpExecSync(void* p1, void* p2, void* p3, void* p4, void* p5, void* p6, void* p7, void* p8);
};
