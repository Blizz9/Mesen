#include "stdafx.h"
#include <algorithm>
#include "NotificationManager.h"

OpExecSyncCallback _opExecSyncCallback;

void NotificationManager::RegisterNotificationListener(shared_ptr<INotificationListener> notificationListener)
{
	auto lock = _lock.AcquireSafe();

	for(weak_ptr<INotificationListener> listener : _listeners) {
		if(listener.lock() == notificationListener) {
			//This listener is already registered, do nothing
			return;
		}
	}

	_listeners.push_back(notificationListener);
}

void NotificationManager::CleanupNotificationListeners()
{
	auto lock = _lock.AcquireSafe();

	//Remove expired listeners
	_listeners.erase(
		std::remove_if(
			_listeners.begin(),
			_listeners.end(),
			[](weak_ptr<INotificationListener> ptr) { return ptr.expired(); }
		),
		_listeners.end()
	);
}

void NotificationManager::SendNotification(ConsoleNotificationType type, void* parameter)
{
	vector<weak_ptr<INotificationListener>> listeners;
	{
		auto lock = _lock.AcquireSafe();
		CleanupNotificationListeners();
		listeners = _listeners;
	}

	//Iterate on a copy without using a lock
	for(weak_ptr<INotificationListener> notificationListener : listeners) {
		shared_ptr<INotificationListener> listener = notificationListener.lock();
		if(listener) {
			listener->ProcessNotification(type, parameter);
		}
	}
}

void NotificationManager::RegisterOpExecSync(OpExecSyncCallback callback)
{
	_opExecSyncCallback = callback;
}

void NotificationManager::OpExecSync(void* p1, void* p2, void* p3, void* p4, void* p5, void* p6, void* p7, void* p8)
{
	_opExecSyncCallback(p1, p2, p3, p4, p5, p6, p7, p8);
}
