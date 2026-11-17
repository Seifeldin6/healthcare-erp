import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';

class OnboardingPageIndicators extends StatelessWidget {
  final int currentIndex;
  final int count;

  const OnboardingPageIndicators({
    super.key,
    required this.currentIndex,
    this.count = 3,
  });

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: List.generate(count, (index) {
        final isActive = index == currentIndex;
        return AnimatedContainer(
          duration: const Duration(milliseconds: 250),
          curve: Curves.easeInOut,
          margin: EdgeInsetsDirectional.only(
            end: index == count - 1 ? 0 : AppDimensions.spacing6.w,
          ),
          width: isActive ? 24.w : 8.w,
          height: 8.h,
          decoration: BoxDecoration(
            color: isActive ? AppColors.primary : AppColors.indicatorInactive,
            borderRadius: BorderRadius.circular(4.r),
          ),
        );
      }),
    );
  }
}