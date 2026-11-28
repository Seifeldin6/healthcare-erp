import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';
import 'package:lucide_icons_flutter/lucide_icons.dart';

class PatientBottomNavBar extends StatelessWidget {
  final int currentIndex;
  final ValueChanged<int> onDestinationSelected;

  const PatientBottomNavBar({
    super.key,
    required this.currentIndex,
    required this.onDestinationSelected,
  });

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      padding: EdgeInsets.only(
        top: 8.h,
        left: 8.w,
        right: 8.w,
        bottom: 24.h,
      ),
      decoration: const BoxDecoration(
        color: AppColors.surface,
        border: Border(
          top: BorderSide(color: AppColors.navBorder, width: 1),
        ),
      ),
      child: NavigationBarTheme(
        data: NavigationBarThemeData(
          backgroundColor: AppColors.surface,
          elevation: 0,
          height: 52.h,
          indicatorColor: AppColors.primaryTint,
          indicatorShape: const StadiumBorder(),
          labelBehavior: NavigationDestinationLabelBehavior.alwaysShow,
          iconTheme: WidgetStateProperty.resolveWith((states) {
            final isSelected = states.contains(WidgetState.selected);
            return IconThemeData(
              size: 22.sp,
              color: isSelected ? AppColors.primary : AppColors.muted,
            );
          }),
          labelTextStyle: WidgetStateProperty.resolveWith((states) {
            final isSelected = states.contains(WidgetState.selected);
            return isSelected
                ? AppTextStyles.caption12PrimaryBold
                : AppTextStyles.caption12MutedSemiBold;
          }),
        ),
        child: NavigationBar(
          selectedIndex: currentIndex,
          onDestinationSelected: onDestinationSelected,
          destinations: [
            NavigationDestination(
              icon: const Icon(LucideIcons.house),
              selectedIcon: const Icon(LucideIcons.house),
              label: s.navHome,
            ),
            NavigationDestination(
              icon: const Icon(LucideIcons.userPlus),
              selectedIcon: const Icon(LucideIcons.userPlus),
              label: s.navPharmacists,
            ),
            NavigationDestination(
              icon: const Icon(LucideIcons.messageCircle),
              selectedIcon: const Icon(LucideIcons.messageCircle),
              label: s.navConsultations,
            ),
            NavigationDestination(
              icon: const Icon(LucideIcons.circleUserRound),
              selectedIcon: const Icon(LucideIcons.circleUserRound),
              label: s.navProfile,
            ),
          ],
        ),
      ),
    );
  }
}