#pragma once
#include "stdafx.h"
#include "INotificationListener.h"
#include "../Utilities/SimpleLock.h"

typedef void(__stdcall* OpExecSyncCallback)(void*);

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
	void OpExecSync(void* parameter);
};
