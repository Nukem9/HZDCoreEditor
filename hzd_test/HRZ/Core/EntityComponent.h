#pragma once

#include <span>

#include "../PCore/Common.h"

#include "RTTIObject.h"
#include "RTTI.h"
#include "WeakPtrTarget.h"
#include "Resource.h"

namespace HRZ
{

class Entity;
class NetEntityComponentState;

DECL_RTTI(EntityComponent);
DECL_RTTI(EntityComponentContainer);
DECL_RTTI(EntityComponentRep);
DECL_RTTI(EntityComponentResource);

class EntityComponentResource : public Resource
{
public:
	TYPE_RTTI(EntityComponentResource);

	virtual const RTTI *GetRTTI() const override;		// 0
	virtual ~EntityComponentResource() override;		// 1
	virtual const RTTI *GetComponentRTTI() const;		// 18
	virtual const RTTI *GetComponentRepRTTI() const;	// 19
	virtual bool EntityComponentResourceUnknown20();	// 20
};
assert_size(EntityComponentResource, 0x28);

class EntityComponentRep : public RTTIObject, public WeakPtrTarget
{
public:
	TYPE_RTTI(EntityComponentRep);

	uint16_t m_Unknown18 = 0;
	void *m_Unknown20 = nullptr;
	void *m_Unknown28 = nullptr;

	virtual const RTTI *GetRTTI() const override;	// 0
	virtual ~EntityComponentRep() override;			// 1
};
assert_offset(EntityComponentRep, m_Unknown18, 0x18);
assert_offset(EntityComponentRep, m_Unknown20, 0x20);
assert_offset(EntityComponentRep, m_Unknown28, 0x28);
assert_size(EntityComponentRep, 0x30);

class EntityComponent : public RTTIRefObject, public WeakPtrTarget
{
public:
	TYPE_RTTI(EntityComponent);

	Ref<EntityComponentResource> m_Resource;	// 0x30
	bool m_Unknown38 = false;					// 0x38
	void *m_Unknown40 = nullptr;				// 0x40
	Entity *m_Entity = nullptr;					// 0x48
	uint32_t m_Unknown50 = -1;					// 0x50 Component index?

	virtual const RTTI *GetRTTI() const override;						// 0
	virtual ~EntityComponent() override;								// 1
	virtual const RTTI *GetComponentRepRTTI() const;					// 4
	virtual void SetEntity(Entity *Entity);								// 5
	virtual void SetResource(Ref<EntityComponentResource> Resource);	// 6
	virtual void EntityComponentUnknown07();							// 7
	virtual void EntityComponentUnknown08();							// 8
	virtual NetEntityComponentState *CreateNetState();					// 9

	String GetUnderlyingName() const
	{
		if (m_Resource)
			return m_Resource->GetName();

		return GetRTTI()->AsClass()->m_Name;
	}
};
assert_offset(EntityComponent, m_Resource, 0x30);
assert_offset(EntityComponent, m_Unknown38, 0x38);
assert_offset(EntityComponent, m_Unknown40, 0x40);
assert_offset(EntityComponent, m_Entity, 0x48);
assert_offset(EntityComponent, m_Unknown50, 0x50);
assert_size(EntityComponent, 0x58);

class EntityComponentContainer
{
public:
	TYPE_RTTI(EntityComponentContainer);

	Array<EntityComponent *> m_Components;		// Grouped by component type
	Array<RTTI::TypeId> m_ComponentRTTITypes;	// Sorted for quick binary searches. Each entry index corresponds to a m_Components entry.

	bool GetFirstComponentIndexByType(const RTTI *Type, size_t& Index) const
	{
		// Binary search on array by finding the lowest bound first (multiple entries with the same type are possible)
		ptrdiff_t lowBound = 0;
		ptrdiff_t highBound = m_ComponentRTTITypes.size();

		while (highBound > 0)
		{
			auto mid = lowBound + (highBound / 2);

			if (m_ComponentRTTITypes[mid] < Type->m_RuntimeTypeId1)
			{
				lowBound = mid + 1;
				highBound = -1 - (highBound / 2) + highBound;
			}
			else
			{
				highBound = highBound / 2;
			}
		}

		Index = lowBound;
		return m_Components[Index]->GetRTTI()->IsKindOf(Type);
	}

	EntityComponent *FindComponentByType(const RTTI *Type) const
	{
		if (size_t index; GetFirstComponentIndexByType(Type, index))
			return m_Components[index];

		return nullptr;
	}

	EntityComponent *FindComponentByResourceName(const String& Name) const
	{
		for (auto& component : m_Components)
		{
			if (!component->m_Resource)
				continue;

			if (component->m_Resource->GetName() == Name)
				return component;
		}

		return nullptr;
	}

	template<typename T>
	T *FindComponent() const
	{
		return static_cast<T *>(FindComponentByType(T::TypeInfo));
	}

	std::span<EntityComponent *> FindComponentsByType(const RTTI *Type)
	{
		__debugbreak();
		__assume(0);
		/*
		if (size_t firstIndex; GetFirstComponentIndexByType(Type, firstIndex))
		{
			// Found first valid index. Now find the last instance in the array.
			auto lastIndex = [&]()
			{
				size_t i = firstIndex;

				for (; i < m_ComponentRTTITypes.size(); i++)
				{
					if (!Type->IsKindOf(m_ComponentRTTITypes[i]))
						break;
				}

				return i;
			}();

			return std::span { &m_Components.data()[firstIndex], &m_Components.data()[lastIndex] };
		}

		return {};*/
	}
};
assert_offset(EntityComponentContainer, m_Components, 0x0);
assert_offset(EntityComponentContainer, m_ComponentRTTITypes, 0x10);
assert_size(EntityComponentContainer, 0x20);

}