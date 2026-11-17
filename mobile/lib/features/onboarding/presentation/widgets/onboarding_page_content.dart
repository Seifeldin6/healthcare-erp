import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'onboarding_page_indicators.dart';

class OnboardingPageContent extends StatelessWidget {
  final Widget mockupCard;
  final int currentIndex;
  final String title;
  final String description;

  const OnboardingPageContent({
    super.key,
    required this.mockupCard,
    required this.currentIndex,
    required this.title,
    required this.description,
  });

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      physics: const NeverScrollableScrollPhysics(),
      padding: EdgeInsetsDirectional.symmetric(
        horizontal: AppDimensions.spacing24.w,
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(height: AppDimensions.spacing16.h),
          mockupCard,
          SizedBox(height: 30.h),
          Center(child: OnboardingPageIndicators(currentIndex: currentIndex)),
          SizedBox(height: AppDimensions.spacing24.h),
          Text(title, style: AppTextStyles.display30InkBold),
          SizedBox(height: AppDimensions.spacing12.h),
          Text(description, style: AppTextStyles.body16BodyRegular),
        ],
      ),
    );
  }
}